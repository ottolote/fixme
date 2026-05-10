using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text.Json;
using FixMe.Access.Customer.Service;
using FixMe.Access.Equipment.Service;
using FixMe.Manager.Membership.Interface;
using FixMe.Manager.Membership.Service;

namespace FixMe.Api;

public sealed class FixMeApi(string[] args, IMembershipManager? membershipManager = null)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly IMembershipManager _membershipManager = membershipManager ?? CreateDefaultMembershipManager();
    private readonly CancellationTokenSource _stopping = new();
    private readonly HttpListener _listener = new();

    public static FixMeApi Create(string[] args) => new(args);

    public static FixMeApi Create(string[] args, IMembershipManager membershipManager) => new(args, membershipManager);

    public string Url { get; } = ResolveUrl(args);

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        using CancellationTokenSource linkedStopping = CancellationTokenSource.CreateLinkedTokenSource(_stopping.Token, cancellationToken);

        _listener.Prefixes.Add(Url);
        _listener.Start();
        Console.WriteLine($"FixMe API listening on {Url}");

        try
        {
            while (!linkedStopping.IsCancellationRequested)
            {
                HttpListenerContext context = await _listener.GetContextAsync().WaitAsync(linkedStopping.Token);
                _ = Task.Run(() => HandleRequestAsync(context), linkedStopping.Token);
            }
        }
        catch (OperationCanceledException) when (linkedStopping.IsCancellationRequested)
        {
        }
        finally
        {
            _listener.Stop();
        }
    }

    public void Stop() => _stopping.Cancel();

    private async Task HandleRequestAsync(HttpListenerContext context)
    {
        try
        {
            await RouteAsync(context);
        }
        catch (Exception exception)
        {
            await WriteJsonAsync(context.Response, HttpStatusCode.InternalServerError, new
            {
                error = "unexpected_error",
                detail = exception.Message
            });
        }
        finally
        {
            context.Response.Close();
        }
    }

    private Task RouteAsync(HttpListenerContext context)
    {
        string method = context.Request.HttpMethod;
        string path = context.Request.Url?.AbsolutePath.TrimEnd('/') ?? string.Empty;
        if (path.Length == 0)
        {
            path = "/";
        }

        return (method, path) switch
        {
            ("GET", "/health/live") => WriteJsonAsync(context.Response, HttpStatusCode.OK, new HealthResponse("ok")),
            ("GET", "/health/ready") => WriteJsonAsync(context.Response, HttpStatusCode.OK, new HealthResponse("ready")),
            ("GET", "/api/v1") => WriteJsonAsync(context.Response, HttpStatusCode.OK, ApiDescription.Current()),
            ("POST", "/api/v1/equipment-registrations/drafts") => CreateDraftRegistrationAsync(context),
            ("GET", "/") => WriteJsonAsync(context.Response, HttpStatusCode.OK, ApiDescription.Current()),
            _ => WriteJsonAsync(context.Response, HttpStatusCode.NotFound, new { error = "not_found" })
        };
    }

    private async Task CreateDraftRegistrationAsync(HttpListenerContext context)
    {
        CreatePendingRegistrationRequest? request;
        try
        {
            request = await JsonSerializer.DeserializeAsync<CreatePendingRegistrationRequest>(context.Request.InputStream, JsonOptions);
        }
        catch (JsonException)
        {
            await WriteJsonAsync(context.Response, HttpStatusCode.BadRequest, new ErrorResponse("invalid-json"));
            return;
        }

        if (request is null)
        {
            await WriteJsonAsync(context.Response, HttpStatusCode.BadRequest, new ErrorResponse("invalid-request"));
            return;
        }

        PendingRegistration registration = await _membershipManager.CreatePendingRegistration(request);
        if (!string.IsNullOrWhiteSpace(registration.Error))
        {
            await WriteJsonAsync(context.Response, MapRegistrationError(registration.Error), new ErrorResponse(registration.Error));
            return;
        }

        await WriteJsonAsync(context.Response, HttpStatusCode.Created, registration);
    }

    private static HttpStatusCode MapRegistrationError(string error)
    {
        return error switch
        {
            "invalid-customer" => HttpStatusCode.NotFound,
            "invalid-equipment-type" => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.BadRequest
        };
    }

    private static async Task WriteJsonAsync<T>(HttpListenerResponse response, HttpStatusCode statusCode, T body)
    {
        response.StatusCode = (int)statusCode;
        response.ContentType = "application/json; charset=utf-8";

        await JsonSerializer.SerializeAsync(response.OutputStream, body, JsonOptions);
    }

    private static string ResolveUrl(string[] args)
    {
        string? explicitUrl = args.FirstOrDefault(arg => arg.StartsWith("--url=", StringComparison.OrdinalIgnoreCase));
        string? configuredUrl = explicitUrl?["--url=".Length..]
            ?? Environment.GetEnvironmentVariable("FIXME_API_URL")
            ?? Environment.GetEnvironmentVariable("ASPNETCORE_URLS")
            ?? "http://*:8080";

        return configuredUrl.EndsWith('/') ? configuredUrl : configuredUrl + "/";
    }

    private static IMembershipManager CreateDefaultMembershipManager()
    {
        return new MembershipManager(new CustomerAccess(), new EquipmentAccess());
    }

    private sealed record HealthResponse(string Status);

    private sealed record ErrorResponse(string Error);

    private sealed record ApiDescription(
        string Name,
        string Version,
        string Environment,
        string TraceId,
        string[] Capabilities)
    {
        public static ApiDescription Current()
        {
            return new ApiDescription(
                "FixMe API",
                Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "0.0.0",
                System.Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production",
                Activity.Current?.TraceId.ToString() ?? Guid.NewGuid().ToString("N"),
                [
                    "membership",
                    "draft-equipment-registration",
                    "maintenance",
                    "notification",
                    "tasking"
                ]);
        }
    }
}
