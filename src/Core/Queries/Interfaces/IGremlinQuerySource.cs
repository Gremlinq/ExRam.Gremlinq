namespace ExRam.Gremlinq.Core
{
    /// <summary>The entry point for building Gremlin queries. Provides environment configuration and side-effect registration.</summary>
    public interface IGremlinQuerySource : IStartGremlinQuery
    {
        /// <summary>
        /// Configures the query environment for this source.
        /// </summary>
        /// <param name="environmentTransformation">A function that transforms the current environment.</param>
        IGremlinQuerySource ConfigureEnvironment(Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> environmentTransformation);

        /// <summary>
        /// Adds a side-effect to the traversal source, making it available to traversal steps via the provided label.
        /// Corresponds to the Gremlin <c>withSideEffect()</c> modulator.
        /// </summary>
        /// <typeparam name="TSideEffect">The type of the side-effect value.</typeparam>
        /// <param name="label">The step label under which the side-effect is stored.</param>
        /// <param name="value">The side-effect value.</param>
        IGremlinQuerySource WithSideEffect<TSideEffect>(StepLabel<TSideEffect> label, TSideEffect value);

        /// <summary>
        /// Adds a side-effect to the traversal source and provides the generated label to a continuation.
        /// Corresponds to the Gremlin <c>withSideEffect()</c> modulator.
        /// </summary>
        /// <typeparam name="TSideEffect">The type of the side-effect value.</typeparam>
        /// <typeparam name="TQuery">The type of query returned by the continuation.</typeparam>
        /// <param name="value">The side-effect value.</param>
        /// <param name="continuation">A function that receives the query source and the generated step label.</param>
        TQuery WithSideEffect<TSideEffect, TQuery>(TSideEffect value, Func<IGremlinQuerySource, StepLabel<TSideEffect>, TQuery> continuation)
            where TQuery : IGremlinQueryBase;
    }
}
