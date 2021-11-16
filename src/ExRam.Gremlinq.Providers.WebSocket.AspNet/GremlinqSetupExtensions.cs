// ReSharper disable HeapView.PossibleBoxingAllocation
using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq.Core.AspNet
{
    public static class GremlinqSetupExtensions
    {
        public static ProviderSetup<TConfigurator> ConfigureWebSocket<TConfigurator>(this ProviderSetup<TConfigurator> setup)
           where TConfigurator : IWebSocketProviderConfigurator<TConfigurator>
        {
            return setup
                .Configure((configurator, gremlinqSection, providerSection) => configurator
                    .ConfigureWebSocket(webSocketConfigurator => webSocketConfigurator
                        .ConfigureFrom(gremlinqSection)
                        .ConfigureFrom(providerSection)));
        }
    }
}
