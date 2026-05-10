using System.Net;
using System.Net.Http.Json;
using FixMe.Api;

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
        Assert.Contains("maintenance", body?.Capabilities ?? []);
        Assert.Contains("notification", body?.Capabilities ?? []);
        Assert.Contains("tasking", body?.Capabilities ?? []);
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

        public static async Task<TestApiHost> StartAsync()
        {
            int port = Random.Shared.Next(20000, 60000);
            FixMeApi api = FixMeApi.Create([$"--url=http://127.0.0.1:{port}"]);
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
}
