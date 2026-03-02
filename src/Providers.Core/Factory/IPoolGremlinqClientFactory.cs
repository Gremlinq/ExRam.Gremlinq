namespace ExRam.Gremlinq.Providers.Core
{
    /// <summary>
    /// A client factory that pools connections to the server.
    /// </summary>
    /// <typeparam name="TBaseFactory">The type of the underlying client factory whose connections are pooled.</typeparam>
    public interface IPoolGremlinqClientFactory<TBaseFactory> : IGremlinqClientFactory<IPoolGremlinqClientFactory<TBaseFactory>>
        where TBaseFactory : IGremlinqClientFactory
    {
        /// <summary>
        /// Configures the underlying base factory by applying a transformation.
        /// </summary>
        /// <typeparam name="TNewBaseFactory">The new base factory type.</typeparam>
        /// <param name="transformation">A function that transforms the current base factory.</param>
        IPoolGremlinqClientFactory<TNewBaseFactory> ConfigureBaseFactory<TNewBaseFactory>(Func<TBaseFactory, TNewBaseFactory> transformation)
            where TNewBaseFactory : IGremlinqClientFactory;

        /// <summary>
        /// Sets the number of connections in the pool.
        /// </summary>
        /// <param name="poolSize">The pool size (1–8).</param>
        IPoolGremlinqClientFactory<TBaseFactory> WithPoolSize(int poolSize);

        /// <summary>
        /// Sets the maximum number of in-process requests per connection.
        /// </summary>
        /// <param name="maxInProcessPerConnection">The maximum number of concurrent requests per connection (1–64).</param>
        IPoolGremlinqClientFactory<TBaseFactory> WithMaxInProcessPerConnection(int maxInProcessPerConnection);
    }
}
