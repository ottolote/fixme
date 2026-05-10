using Autofac;
using SoEx;

namespace FixMe.Host.InProc
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var hosts = FixMe.iFx.Hosting.Host.InProc(args)
                .WithTestClient(TestClient, shutdownWhenCompleted: true);
            await hosts.RunAsync();
        }

        private static async Task TestClient(ILifetimeScope lifetimeScope)
        {
            using (var requestScope = lifetimeScope.BeginLifetimeScopeAsyncLocal())
            {
                // var proxy = Proxy.ForService<I_XXX_Manager>();
                // await proxy._Operation_();
            }
            await Task.Delay(1000);
        }
    }
}
