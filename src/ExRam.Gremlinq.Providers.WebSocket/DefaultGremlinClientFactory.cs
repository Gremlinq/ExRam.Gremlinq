using System;
using System.Net.WebSockets;

using Gremlin.Net.Driver;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public sealed class DefaultGremlinClientFactory : IGremlinClientFactory
    {
        public GremlinClient Create(Gremlin.Net.Driver.GremlinServer gremlinServer, IMessageSerializer? messageSerializer = null, ConnectionPoolSettings? connectionPoolSettings = null, Action<ClientWebSocketOptions>? webSocketConfiguration = null, string? sessionId = null)
        {
            return new GremlinClient(
                gremlinServer,
                messageSerializer,
                connectionPoolSettings,
                webSocketConfiguration,
                sessionId);
        }
    }
}
