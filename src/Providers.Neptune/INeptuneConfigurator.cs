using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Providers.Neptune
{
    /// <summary>
    /// A configurator for AWS Neptune Gremlin connections.
    /// </summary>
    public interface INeptuneConfigurator : IProviderConfigurator<INeptuneConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>>;
}
