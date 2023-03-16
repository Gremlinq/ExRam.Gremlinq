using ExRam.Gremlinq.Providers.Core;
using _GremlinServer = Gremlin.Net.Driver.GremlinServer;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public interface IWebSocketProviderConfigurator<out TConfigurator> : IProviderConfigurator<TConfigurator>
        where TConfigurator : IProviderConfigurator<TConfigurator>
    {
        TConfigurator ConfigureServer(Func<_GremlinServer, _GremlinServer> transformation);

        TConfigurator ConfigureClientFactory(Func<IGremlinClientFactory, IGremlinClientFactory> transformation);
    }
}
