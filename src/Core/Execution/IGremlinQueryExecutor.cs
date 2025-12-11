namespace ExRam.Gremlinq.Core.Execution
{
    /// <summary>
    /// Executes Gremlin queries against a graph database and returns the results.
    /// </summary>
    public interface IGremlinQueryExecutor
    {
        /// <summary>
        /// Executes a Gremlin query and returns the results as an asynchronous stream.
        /// </summary>
        /// <typeparam name="T">The expected type of the query results.</typeparam>
        /// <param name="context">The execution context containing the query and execution metadata.</param>
        /// <returns>An asynchronous enumerable of query results.</returns>
        IAsyncEnumerable<T> Execute<T>(GremlinQueryExecutionContext context);
    }
}
