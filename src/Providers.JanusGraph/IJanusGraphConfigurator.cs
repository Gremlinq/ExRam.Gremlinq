using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Providers.JanusGraph
{
    /// <summary>Configurator interface for JanusGraph provider settings.</summary>
    public interface IJanusGraphConfigurator : IProviderConfigurator<IJanusGraphConfigurator, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>>;
}
