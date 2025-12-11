namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Represents the source for creating Gremlin queries, providing the entry point for query construction.
    /// </summary>
    public interface IGremlinQuerySource : IStartGremlinQuery
    {
        /// <summary>
        /// Configures the query environment by applying a transformation function.
        /// </summary>
        /// <param name="environmentTransformation">A function that transforms the query environment.</param>
        /// <returns>A new query source with the transformed environment.</returns>
        IGremlinQuerySource ConfigureEnvironment(Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> environmentTransformation);

        /// <summary>
        /// Adds a side effect with a specific label to the query source.
        /// </summary>
        /// <typeparam name="TSideEffect">The type of the side effect value.</typeparam>
        /// <param name="label">The label identifying the side effect.</param>
        /// <param name="value">The side effect value.</param>
        /// <returns>A new query source with the side effect added.</returns>
        IGremlinQuerySource WithSideEffect<TSideEffect>(StepLabel<TSideEffect> label, TSideEffect value);

        /// <summary>
        /// Adds a side effect and continues query construction with a continuation function.
        /// </summary>
        /// <typeparam name="TSideEffect">The type of the side effect value.</typeparam>
        /// <typeparam name="TQuery">The type of query to construct.</typeparam>
        /// <param name="value">The side effect value.</param>
        /// <param name="continuation">A function that continues query construction with access to the side effect label.</param>
        /// <returns>The query constructed by the continuation function.</returns>
        TQuery WithSideEffect<TSideEffect, TQuery>(TSideEffect value, Func<IGremlinQuerySource, StepLabel<TSideEffect>, TQuery> continuation)
            where TQuery : IGremlinQueryBase;
    }
}
