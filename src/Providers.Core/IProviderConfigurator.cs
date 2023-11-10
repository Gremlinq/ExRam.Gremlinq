using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Providers.Core
{
    public interface IProviderConfigurator<out TSelf, TClientFactory> : IGremlinqConfigurator<TSelf>
        where TSelf : IGremlinqConfigurator<TSelf>
        where TClientFactory : IGremlinqClientFactory
    {
        TSelf ConfigureClientFactory(Func<IGremlinqClientFactory, IGremlinqClientFactory> transformation);
    }
}
