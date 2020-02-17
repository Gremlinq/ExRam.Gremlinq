using System;

namespace ExRam.Gremlinq.Core
{
    public static class WebSocketQuerySourceBuilder
    {
        public static IWebSocketConfigurationBuilder AtLocalhost(this IWebSocketConfigurationBuilder builder)
        {
            return builder.At(new Uri("ws://localhost:8182"));
        }
    }
}