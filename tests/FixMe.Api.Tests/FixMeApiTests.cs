using System.Net;
using System.Net.Http.Json;
using FixMe.Api;
using FixMe.Manager.Membership.Interface;

namespace FixMe.Api.Tests;

public sealed class FixMeApiTests
{
    [Fact]
    public async Task HealthLiveReturnsOk()
    {
        await using TestApiHost host = await TestApiHost.StartAsync();

        using HttpResponseMessage response = await host.Client.GetAsync("/health/live");
        HealthResponse? body = await response.Content.ReadFromJsonAsync<HealthResponse>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("ok", body?.Status);
    }

    [Fact]
    public async Task ApiRootDescribesInitialCapabilities()
    {
        await using TestApiHost host = await TestApiHost.StartAsync();

        ApiDescription? body = await host.Client.GetFromJsonAsync<ApiDescription>("/api/v1");

        Assert.Equal("FixMe API", body?.Name);
        Assert.Contains("membership", body?.Capabilities ?? []);
        Assert.Contains("draft-equipment-registration", body?.Capabilities ?? []);
        Assert.Contains("maintenance", body?.Capabilities ?? []);
        Assert.Contains("notification", body?.Capabilities ?? []);
        Assert.Contains("tasking", body?.Capabilities ?? []);
    }

    [Fact]
    public async Task CreateDraftRegistrationReturnsCreatedRegistration()
    {
        FakeMembershipManager membershipManager = new(new PendingRegistration
        {
            PendingRegistrationId = "draft-123",
            CustomerId = "customer-123",
            EquipmentTypeId = "car",
            Status = "Draft",
            EventName = "Pending equipment registration created"
        });
        await using TestApiHost host = await TestApiHost.StartAsync(membershipManager);

        using HttpResponseMessage response = await host.Client.PostAsJsonAsync(
            "/api/v1/equipment-registrations/drafts",
            new { CustomerId = "customer-123", EquipmentTypeId = "car" });
        PendingRegistration? body = await response.Content.ReadFromJsonAsync<PendingRegistration>();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal("draft-123", body?.PendingRegistrationId);
        Assert.Equal("customer-123", body?.CustomerId);
        Assert.Equal("car", body?.EquipmentTypeId);
        Assert.Equal("Draft", body?.Status);
        Assert.Equal("Pending equipment registration created", body?.EventName);
        Assert.Equal("customer-123", membershipManager.LastRequest?.CustomerId);
        Assert.Equal("car", membershipManager.LastRequest?.EquipmentTypeId);
        Assert.Equal(1, membershipManager.CreatePendingRegistrationCalls);
    }

    [Fact]
    public async Task CreateDraftRegistrationReturnsNotFoundForInvalidCustomer()
    {
        FakeMembershipManager membershipManager = new(new PendingRegistration { Error = "invalid-customer" });
        await using TestApiHost host = await TestApiHost.StartAsync(membershipManager);

        using HttpResponseMessage response = await host.Client.PostAsJsonAsync(
            "/api/v1/equipment-registrations/drafts",
            new { CustomerId = "missing-customer" });
        ErrorResponse? body = await response.Content.ReadFromJsonAsync<ErrorResponse>();

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("invalid-customer", body?.Error);
        Assert.Equal("missing-customer", membershipManager.LastRequest?.CustomerId);
        Assert.Equal(1, membershipManager.CreatePendingRegistrationCalls);
    }

    [Fact]
    public async Task CreateDraftRegistrationReturnsBadRequestForInvalidEquipmentType()
    {
        FakeMembershipManager membershipManager = new(new PendingRegistration { Error = "invalid-equipment-type" });
        await using TestApiHost host = await TestApiHost.StartAsync(membershipManager);

        using HttpResponseMessage response = await host.Client.PostAsJsonAsync(
            "/api/v1/equipment-registrations/drafts",
            new { CustomerId = "customer-123", EquipmentTypeId = "unknown" });
        ErrorResponse? body = await response.Content.ReadFromJsonAsync<ErrorResponse>();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("invalid-equipment-type", body?.Error);
        Assert.Equal("unknown", membershipManager.LastRequest?.EquipmentTypeId);
        Assert.Equal(1, membershipManager.CreatePendingRegistrationCalls);
    }

    [Fact]
    public async Task CreateDraftRegistrationReturnsBadRequestForMalformedJson()
    {
        FakeMembershipManager membershipManager = new(new PendingRegistration
        {
            PendingRegistrationId = "should-not-be-created"
        });
        await using TestApiHost host = await TestApiHost.StartAsync(membershipManager);

        using HttpResponseMessage response = await host.Client.PostAsync(
            "/api/v1/equipment-registrations/drafts",
            new StringContent("{", System.Text.Encoding.UTF8, "application/json"));
        ErrorResponse? body = await response.Content.ReadFromJsonAsync<ErrorResponse>();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("invalid-json", body?.Error);
        Assert.Equal(0, membershipManager.CreatePendingRegistrationCalls);
    }

    private sealed class TestApiHost : IAsyncDisposable
    {
        private readonly Task _runTask;
        private readonly FixMeApi _api;

        private TestApiHost(FixMeApi api, Task runTask)
        {
            _api = api;
            _runTask = runTask;
            Client = new HttpClient { BaseAddress = new Uri(api.Url.Replace("0.0.0.0", "127.0.0.1", StringComparison.Ordinal)) };
        }

        public HttpClient Client { get; }

        public static async Task<TestApiHost> StartAsync(IMembershipManager? membershipManager = null)
        {
            int port = Random.Shared.Next(20000, 60000);
            FixMeApi api = membershipManager is null
                ? FixMeApi.Create([$"--url=http://127.0.0.1:{port}"])
                : FixMeApi.Create([$"--url=http://127.0.0.1:{port}"], membershipManager);
            Task runTask = Task.Run(() => api.RunAsync());
            TestApiHost host = new(api, runTask);

            using CancellationTokenSource timeout = new(TimeSpan.FromSeconds(10));
            while (!timeout.IsCancellationRequested)
            {
                try
                {
                    using HttpResponseMessage response = await host.Client.GetAsync("/health/live", timeout.Token);
                    if (response.IsSuccessStatusCode)
                    {
                        return host;
                    }
                }
                catch (HttpRequestException)
                {
                    await Task.Delay(50, timeout.Token);
                }
            }

            throw new TimeoutException("FixMe API did not start within 10 seconds.");
        }

        public async ValueTask DisposeAsync()
        {
            Client.Dispose();
            _api.Stop();
            await _runTask.WaitAsync(TimeSpan.FromSeconds(5));
        }
    }

    private sealed record HealthResponse(string Status);

    private sealed record ApiDescription(string Name, string[] Capabilities);

    private sealed record ErrorResponse(string Error);

    private sealed class FakeMembershipManager(PendingRegistration response) : IMembershipManager
    {
        public int CreatePendingRegistrationCalls { get; private set; }

        public CreatePendingRegistrationRequest? LastRequest { get; private set; }

        public Task<RegisterAccountResponse> RegisterAccount(RegisterAccountRequest request) => throw new NotSupportedException();

        public Task<ConfirmUserEmailResponse> ConfirmUserEmail(ConfirmUserEmailRequest request) => throw new NotSupportedException();

        public Task<UpdateUserPasswordResponse> UpdateUserPassword(UpdateUserPasswordRequest request) => throw new NotSupportedException();

        public Task<SetUserPreferencesResponse> SetUserPreferences(SetUserPreferencesRequest request) => throw new NotSupportedException();

        public Task<PendingRegistration> CreatePendingRegistration(CreatePendingRegistrationRequest request)
        {
            LastRequest = request;
            CreatePendingRegistrationCalls++;

            return Task.FromResult(response);
        }

        public Task<ResolvePendingRegistrationResponse> ResolvePendingRegistration(ResolvePendingRegistrationRequest request) => throw new NotSupportedException();
    }
}
