namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// A configurator that can transform a query source and supports fluent chaining.
    /// Used by provider packages to configure database-specific settings.
    /// </summary>
    /// <typeparam name="TSelf">The concrete configurator type for fluent chaining.</typeparam>
    public interface IGremlinqConfigurator<out TSelf> : IGremlinQuerySourceTransformation
        where TSelf : IGremlinqConfigurator<TSelf>
    {
        /// <summary>
        /// Configures the underlying query source by applying a transformation.
        /// </summary>
        /// <param name="transformation">A function that transforms the query source.</param>
        TSelf ConfigureQuerySource(Func<IGremlinQuerySource, IGremlinQuerySource> transformation);
    }
}
