namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Provides configuration capabilities for Gremlinq query sources with a fluent interface.
    /// </summary>
    /// <typeparam name="TSelf">The concrete type of the configurator, enabling fluent method chaining.</typeparam>
    public interface IGremlinqConfigurator<out TSelf> : IGremlinQuerySourceTransformation
        where TSelf : IGremlinqConfigurator<TSelf>
    {
        /// <summary>
        /// Configures the query source by applying a transformation function.
        /// </summary>
        /// <param name="transformation">A function that transforms the query source.</param>
        /// <returns>The configurator instance for method chaining.</returns>
        TSelf ConfigureQuerySource(Func<IGremlinQuerySource, IGremlinQuerySource> transformation);
    }
}
