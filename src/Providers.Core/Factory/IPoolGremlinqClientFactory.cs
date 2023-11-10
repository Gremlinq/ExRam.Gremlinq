namespace ExRam.Gremlinq.Providers.Core
{
    public interface IPoolGremlinqClientFactory<TBaseFactory> : IGremlinqClientFactory
        where TBaseFactory : IGremlinqClientFactory
    {
        IPoolGremlinqClientFactory<TBaseFactory> ConfigureBaseFactory(Func<TBaseFactory, TBaseFactory> transformation);

        IPoolGremlinqClientFactory<TBaseFactory> WithPoolSize(uint poolSize);

        IPoolGremlinqClientFactory<TBaseFactory> WithMaxInProcessPerConnection(uint maxInProcessPerConnection);
    }
}
