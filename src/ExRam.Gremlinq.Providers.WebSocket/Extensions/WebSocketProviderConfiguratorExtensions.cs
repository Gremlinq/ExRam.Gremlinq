using System;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public static class WebSocketProviderConfiguratorExtensions
    {
        public static TConfigurator At<TConfigurator>(this TConfigurator configurator, Uri uri)
            where TConfigurator : IWebSocketProviderConfigurator<TConfigurator>
        {
            return configurator
                .ConfigureWebSocket(wsConfigurator => wsConfigurator
                    .At(uri));
        }

        public static TConfigurator AtLocalhost<TConfigurator>(this TConfigurator configurator)
            where TConfigurator : IWebSocketProviderConfigurator<TConfigurator>
        {
            return configurator
                .At(new Uri("ws://localhost:8182"));
        }
    }
}
