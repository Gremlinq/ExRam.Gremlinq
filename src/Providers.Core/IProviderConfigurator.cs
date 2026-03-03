using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Providers.Core
{
    /// <summary>
    /// A provider configurator that exposes the ability to configure its underlying client factory.
    /// </summary>
    /// <typeparam name="TSelf">The concrete configurator type for fluent chaining.</typeparam>
    /// <typeparam name="TClientFactory">The type of client factory used by this provider.</typeparam>
    public interface IProviderConfigurator<out TSelf, TClientFactory> : IGremlinqConfigurator<TSelf>
        where TSelf : IGremlinqConfigurator<TSelf>
        where TClientFactory : IGremlinqClientFactory
    {
        /// <summary>
        /// Configures the client factory by applying a transformation.
        /// </summary>
        /// <param name="transformation">A function that transforms the client factory.</param>
        TSelf ConfigureClientFactory(Func<TClientFactory, TClientFactory> transformation);
    }
}
