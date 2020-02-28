using System;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public static class WebSocketConfigurationBuilderExtensions
    {
        public static IWebSocketConfigurationBuilder At(this IWebSocketConfigurationBuilder builder, string uri)
        {
            return builder.At(new Uri(uri));
        }
    }
}
