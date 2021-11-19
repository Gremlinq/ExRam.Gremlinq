// ReSharper disable HeapView.PossibleBoxingAllocation
using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq.Providers.Core.AspNet
{
    public static class ProviderSetupExtensions
    {
        public static ProviderSetup<TConfigurator> ConfigureWebSocket<TConfigurator>(this ProviderSetup<TConfigurator> setup)
           where TConfigurator : IWebSocketProviderConfigurator<TConfigurator>
        {
            return setup
                .Configure((configurator, providerSection) => configurator
                    .ConfigureWebSocket(webSocketConfigurator => webSocketConfigurator
                        .ConfigureFrom(providerSection
                            .MergeWithGremlinqSection())));
        }
    }
}
