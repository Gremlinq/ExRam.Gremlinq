namespace ExRam.Gremlinq.Core.Execution
{
    /// <summary>
    /// Represents an exception that occurs during Gremlin query execution.
    /// </summary>
    public class GremlinQueryExecutionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GremlinQueryExecutionException"/> class with an execution context and inner exception.
        /// </summary>
        /// <param name="executionContext">The execution context when the exception occurred.</param>
        /// <param name="innerException">The exception that caused the query execution to fail.</param>
        public GremlinQueryExecutionException(GremlinQueryExecutionContext executionContext, Exception innerException) : this(executionContext, $"Executing query {executionContext.ExecutionId:D} failed.", innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GremlinQueryExecutionException"/> class with an execution context, message, and inner exception.
        /// </summary>
        /// <param name="executionContext">The execution context when the exception occurred.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that caused the query execution to fail.</param>
        public GremlinQueryExecutionException(GremlinQueryExecutionContext executionContext, string message, Exception innerException) : base(message, innerException)
        {
            ExecutionContext = executionContext;
        }

        /// <summary>
        /// Gets the execution context associated with this exception.
        /// </summary>
        public GremlinQueryExecutionContext ExecutionContext { get; }
    }
}
