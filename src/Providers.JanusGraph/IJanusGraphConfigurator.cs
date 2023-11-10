using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Providers.JanusGraph
{
    public interface IJanusGraphConfigurator : IProviderConfigurator<IJanusGraphConfigurator, IGremlinqClientFactory>
    {
    }
}
