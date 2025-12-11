namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Represents a transformation that can be applied to a Gremlinq configurator.
    /// </summary>
    /// <typeparam name="TConfigurator">The type of configurator to transform.</typeparam>
    public interface IGremlinqConfiguratorTransformation<TConfigurator>
        where TConfigurator : IGremlinqConfigurator<TConfigurator>
    {
        /// <summary>
        /// Transforms the specified configurator.
        /// </summary>
        /// <param name="configurator">The configurator to transform.</param>
        /// <returns>The transformed configurator.</returns>
        TConfigurator Transform(TConfigurator configurator);
    }
}
