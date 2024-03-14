using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Providers.Core
{
    public interface IGremlinqClientFactory
    {
        IGremlinqClient Create(IGremlinQueryEnvironment environment);
    }

    public interface IGremlinqClientFactory<TSelf> : IGremlinqClientFactory
        where TSelf : IGremlinqClientFactory<TSelf>
    {
    }
}
