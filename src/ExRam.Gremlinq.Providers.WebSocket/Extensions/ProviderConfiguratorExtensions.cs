using System;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public static class ProviderConfiguratorExtensions
    {
        public static TProviderConfigurator At<TProviderConfigurator>(this TProviderConfigurator configurator, Uri uri)
           where TProviderConfigurator : IWebSocketProviderConfigurator<TProviderConfigurator>
        {
            return configurator
                .ConfigureWebSocket(c => c
                    .At(uri));
        }

        public static TProviderConfigurator AtLocalhost<TProviderConfigurator>(this TProviderConfigurator configurator)
            where TProviderConfigurator : IWebSocketProviderConfigurator<TProviderConfigurator>
        {
            return configurator
                .ConfigureWebSocket(c => c
                    .AtLocalhost());
        }
    }
}
