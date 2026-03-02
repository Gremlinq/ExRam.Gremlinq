using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Providers.Core
{
    /// <summary>
    /// A factory that creates <see cref="IGremlinqClient"/> instances for a given query environment.
    /// </summary>
    public interface IGremlinqClientFactory
    {
        /// <summary>
        /// Creates a new <see cref="IGremlinqClient"/> configured for the specified environment.
        /// </summary>
        /// <param name="environment">The query environment to configure the client for.</param>
        IGremlinqClient Create(IGremlinQueryEnvironment environment);
    }

    /// <summary>
    /// A client factory that supports fluent configuration of created clients.
    /// </summary>
    /// <typeparam name="TSelf">The concrete factory type for fluent chaining.</typeparam>
    public interface IGremlinqClientFactory<TSelf> : IGremlinqClientFactory
        where TSelf : IGremlinqClientFactory<TSelf>
    {
        /// <summary>
        /// Configures created clients by applying a transformation.
        /// </summary>
        /// <param name="clientTransformation">A function that receives the created client and its environment, and returns a transformed client.</param>
        TSelf ConfigureClient(Func<IGremlinqClient, IGremlinQueryEnvironment, IGremlinqClient> clientTransformation);
    }
}
