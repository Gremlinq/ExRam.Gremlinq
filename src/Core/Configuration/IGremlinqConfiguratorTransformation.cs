namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Transforms a configurator of type <typeparamref name="TConfigurator"/>.
    /// Implementations are typically registered in dependency injection to apply provider-specific configuration.
    /// </summary>
    /// <typeparam name="TConfigurator">The type of configurator to transform.</typeparam>
    public interface IGremlinqConfiguratorTransformation<TConfigurator>
        where TConfigurator : IGremlinqConfigurator<TConfigurator>
    {
        /// <summary>
        /// Transforms the specified configurator.
        /// </summary>
        /// <param name="configurator">The configurator to transform.</param>
        TConfigurator Transform(TConfigurator configurator);
    }
}
