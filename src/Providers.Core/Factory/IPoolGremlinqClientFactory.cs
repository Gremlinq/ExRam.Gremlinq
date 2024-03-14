namespace ExRam.Gremlinq.Providers.Core
{
    public interface IPoolGremlinqClientFactory<TBaseFactory> : IGremlinqClientFactory<IPoolGremlinqClientFactory<TBaseFactory>>
        where TBaseFactory : IGremlinqClientFactory
    {
        IPoolGremlinqClientFactory<TNewBaseFactory> ConfigureBaseFactory<TNewBaseFactory>(Func<TBaseFactory, TNewBaseFactory> transformation)
            where TNewBaseFactory : IGremlinqClientFactory;

        IPoolGremlinqClientFactory<TBaseFactory> WithPoolSize(int poolSize);

        IPoolGremlinqClientFactory<TBaseFactory> WithMaxInProcessPerConnection(int maxInProcessPerConnection);
    }
}
