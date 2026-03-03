namespace ExRam.Gremlinq.Core.Execution
{
    /// <summary>
    /// Executes Gremlin queries against a graph database and returns an asynchronous stream of results.
    /// </summary>
    public interface IGremlinQueryExecutor
    {
        /// <summary>
        /// Executes the query described by the specified execution context and returns the results as an asynchronous stream.
        /// </summary>
        /// <typeparam name="T">The type of the result elements.</typeparam>
        /// <param name="context">The execution context containing the query to execute.</param>
        IAsyncEnumerable<T> Execute<T>(GremlinQueryExecutionContext context);
    }
}
