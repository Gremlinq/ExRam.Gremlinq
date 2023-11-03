using System.Net.WebSockets;
using ExRam.Gremlinq.Core;
using Gremlin.Net.Driver;

namespace ExRam.Gremlinq.Providers.Core
{
    public interface IGremlinClientFactory
    {
        IGremlinClientFactory ConfigureServer(Func<GremlinServer, GremlinServer> transformation);

        IGremlinClient Create(IGremlinQueryEnvironment environment, IMessageSerializer messageSerializer, ConnectionPoolSettings connectionPoolSettings, Action<ClientWebSocketOptions> webSocketConfiguration);
    }
}
