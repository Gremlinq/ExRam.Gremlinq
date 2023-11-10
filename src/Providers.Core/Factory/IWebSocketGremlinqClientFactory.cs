using System.Net.WebSockets;

using Gremlin.Net.Driver;

namespace ExRam.Gremlinq.Providers.Core
{
    public interface IWebSocketGremlinqClientFactory : IGremlinqClientFactory
    {
        IWebSocketGremlinqClientFactory ConfigureServer(Func<GremlinServer, GremlinServer> transformation);

        IWebSocketGremlinqClientFactory ConfigureWebSocketOptions(Action<ClientWebSocketOptions> configuration);
    }
}
