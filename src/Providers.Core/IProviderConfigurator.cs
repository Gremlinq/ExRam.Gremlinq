using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Providers.Core
{
    public interface IProviderConfigurator<out TSelf> : IGremlinqConfigurator<TSelf>
        where TSelf : IGremlinqConfigurator<TSelf>
    {
        TSelf ConfigureClientFactory(Func<IGremlinqClientFactory, IGremlinqClientFactory> transformation);
    }
}
