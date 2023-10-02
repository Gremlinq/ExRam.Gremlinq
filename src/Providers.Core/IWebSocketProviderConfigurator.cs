using ExRam.Gremlinq.Core;

using _GremlinServer = Gremlin.Net.Driver.GremlinServer;

namespace ExRam.Gremlinq.Providers.Core
{
    public interface IWebSocketProviderConfigurator<out TConfigurator> : IGremlinqConfigurator<TConfigurator>
        where TConfigurator : IGremlinqConfigurator<TConfigurator>
    {
        TConfigurator ConfigureServer(Func<_GremlinServer, _GremlinServer> transformation);

        TConfigurator ConfigureClientFactory(Func<IGremlinClientFactory, IGremlinClientFactory> transformation);
    }
}
