using ExRam.Gremlinq.Core;

using _GremlinServer = Gremlin.Net.Driver.GremlinServer;

namespace ExRam.Gremlinq.Providers.Core
{
    public interface IWebSocketProviderConfigurator<out TSelf> : IGremlinqConfigurator<TSelf>
        where TSelf : IGremlinqConfigurator<TSelf>
    {
        TSelf ConfigureServer(Func<_GremlinServer, _GremlinServer> transformation);

        TSelf ConfigureClientFactory(Func<IGremlinClientFactory, IGremlinClientFactory> transformation);
    }
}
