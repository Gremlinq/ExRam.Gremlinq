using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Providers.GremlinServer
{
    /// <summary>
    /// A configurator for Apache TinkerPop Gremlin Server connections.
    /// </summary>
    public interface IGremlinServerConfigurator : IProviderConfigurator<IGremlinServerConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>>;
}
