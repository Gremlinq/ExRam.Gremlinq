namespace ExRam.Gremlinq.Providers.Core
{
    /// <summary>
    /// A client factory that pools connections to the server.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A client created by the pool keeps the session requests of <see cref="IGremlinqClient"/> on one connection: the
    /// first session request of an id binds the id to the connection it is sent on, and every later request of that id
    /// is sent on it. The pool size does not limit the number of sessions; several sessions may share a connection, and
    /// <see cref="WithMaxInProcessPerConnection(int)"/> counts their requests like any other.
    /// </para>
    /// <para>
    /// A connection that fails is dropped, and later requests of other sessions or without one get a new connection. A
    /// session bound to the dropped connection fails from then on with an <see cref="InvalidOperationException"/> that
    /// names the session, until a close request of the session releases it. A sessionless request that fails with an
    /// <see cref="ObjectDisposedException"/> is retried once; a session request is never retried. A close request releases
    /// the binding of its session when it is sent, whatever the server answers. Bindings belong to the client and are
    /// released when it is disposed.
    /// </para>
    /// </remarks>
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
