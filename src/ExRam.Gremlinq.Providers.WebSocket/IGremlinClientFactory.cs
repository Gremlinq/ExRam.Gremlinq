using System;
using System.Net.WebSockets;
using Gremlin.Net.Driver;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public interface IGremlinClientFactory
    {
        IGremlinClient Create(GremlinServer gremlinServer, IMessageSerializer messageSerializer, ConnectionPoolSettings connectionPoolSettings, Action<ClientWebSocketOptions> webSocketConfiguration, string? sessionId = null);
    }
}
