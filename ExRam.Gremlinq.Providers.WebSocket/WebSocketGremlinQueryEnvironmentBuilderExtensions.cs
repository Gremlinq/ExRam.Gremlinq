using System;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public static class WebSocketGremlinQueryEnvironmentBuilderExtensions
    {
        public static IWebSocketGremlinQueryEnvironmentBuilder At(this IWebSocketGremlinQueryEnvironmentBuilder builder, string uri)
        {
            return builder.At(new Uri(uri));
        }

        public static IWebSocketGremlinQueryEnvironmentBuilder AtLocalhost(this IWebSocketGremlinQueryEnvironmentBuilder builder)
        {
            return builder.At(new Uri("ws://localhost:8182"));
        }
    }
}
