using System;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public static class WebSocketConfiguratorExtensions
    {
        public static IWebSocketConfigurator At(this IWebSocketConfigurator builder, string uri)
        {
            return builder.At(new Uri(uri));
        }

        public static IWebSocketConfigurator AtLocalhost(this IWebSocketConfigurator builder)
        {
            return builder.At(new Uri("ws://localhost:8182"));
        }
    }
}
