using System;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public static class WebSocketGremlinQueryEnvironmentBuilderExtensions
    {
        public static IWebSocketGremlinQueryExecutorBuilder At(this IWebSocketGremlinQueryExecutorBuilder builder, string uri)
        {
            return builder.At(new Uri(uri));
        }

        public static IWebSocketGremlinQueryExecutorBuilder AtLocalhost(this IWebSocketGremlinQueryExecutorBuilder builder)
        {
            return builder.At(new Uri("ws://localhost:8182"));
        }
    }
}
