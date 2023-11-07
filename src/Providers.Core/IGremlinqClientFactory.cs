using System.Net.WebSockets;
using ExRam.Gremlinq.Core;
using Gremlin.Net.Driver;

namespace ExRam.Gremlinq.Providers.Core
{
    public interface IGremlinqClientFactory
    {
        IGremlinqClientFactory ConfigureServer(Func<GremlinServer, GremlinServer> transformation);

        IGremlinqClientFactory ConfigureWebSocketOptions(Action<ClientWebSocketOptions> configuration);

        IGremlinqClientFactory ConfigureConnectionPool(Action<ConnectionPoolSettings> configuration);

        IGremlinqClient Create(IGremlinQueryEnvironment environment);
    }
}
