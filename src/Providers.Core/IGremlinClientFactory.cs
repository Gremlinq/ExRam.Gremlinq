using System.Net.WebSockets;
using ExRam.Gremlinq.Core;
using Gremlin.Net.Driver;

namespace ExRam.Gremlinq.Providers.Core
{
    public interface IGremlinClientFactory
    {
        IGremlinClientFactory ConfigureServer(Func<GremlinServer, GremlinServer> transformation);

        IGremlinClientFactory ConfigureWebSocketOptions(Action<ClientWebSocketOptions> configuration);

        IGremlinClientFactory ConfigureConnectionPool(Action<ConnectionPoolSettings> configuration);

        IGremlinClient Create(IGremlinQueryEnvironment environment);
    }
}
