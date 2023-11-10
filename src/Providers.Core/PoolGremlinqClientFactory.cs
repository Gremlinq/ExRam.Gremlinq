using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Providers.Core
{
    internal sealed class PoolGremlinqClientFactory<TBaseFactory> : IPoolGremlinqClientFactory<TBaseFactory>
        where TBaseFactory : IGremlinqClientFactory
    {
        private readonly int _poolSize;
        private readonly TBaseFactory _baseFactory;
        private readonly int _maxInProcessPerConnection;

        public PoolGremlinqClientFactory(TBaseFactory baseFactory) : this(baseFactory, 8, 16)
        {
        }

        public PoolGremlinqClientFactory(TBaseFactory baseFactory, int poolSize, int maxInProcessPerConnection)
        {
            _poolSize = poolSize;
            _baseFactory = baseFactory;
            _maxInProcessPerConnection = maxInProcessPerConnection;
        }

        public IPoolGremlinqClientFactory<TBaseFactory> ConfigureBaseFactory(Func<TBaseFactory, TBaseFactory> transformation) => new PoolGremlinqClientFactory<TBaseFactory>(transformation(_baseFactory));

        public IPoolGremlinqClientFactory<TBaseFactory> WithMaxInProcessPerConnection(int maxInProcessPerConnection) => new PoolGremlinqClientFactory<TBaseFactory>(_baseFactory, _poolSize, maxInProcessPerConnection);

        public IPoolGremlinqClientFactory<TBaseFactory> WithPoolSize(int poolSize) => new PoolGremlinqClientFactory<TBaseFactory>(_baseFactory, poolSize, _maxInProcessPerConnection);

        public IGremlinqClient Create(IGremlinQueryEnvironment environment) => new PoolGremlinqClient(_baseFactory, _poolSize, environment);
    }
}
