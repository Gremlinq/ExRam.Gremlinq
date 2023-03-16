using System.Net.WebSockets;
using Gremlin.Net.Driver;

namespace ExRam.Gremlinq.Providers.Core
{
    public interface IGremlinClientFactory
    {
        IGremlinClient Create(GremlinServer gremlinServer, IMessageSerializer messageSerializer, ConnectionPoolSettings connectionPoolSettings, Action<ClientWebSocketOptions> webSocketConfiguration, string? sessionId = null);
    }
}
