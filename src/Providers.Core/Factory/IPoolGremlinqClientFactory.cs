namespace ExRam.Gremlinq.Providers.Core
{
    public interface IPoolGremlinqClientFactory<TBaseFactory> : IGremlinqClientFactory
        where TBaseFactory : IGremlinqClientFactory
    {
        IPoolGremlinqClientFactory<TBaseFactory> ConfigureBaseFactory(Func<TBaseFactory, TBaseFactory> transformation);

        IPoolGremlinqClientFactory<TBaseFactory> WithPoolSize(int poolSize);

        IPoolGremlinqClientFactory<TBaseFactory> WithMaxInProcessPerConnection(int maxInProcessPerConnection);
    }
}
