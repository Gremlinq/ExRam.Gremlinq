namespace ExRam.Gremlinq.Core.Execution
{
    /// <summary>
    /// The exception that is thrown when executing a Gremlin query fails.
    /// </summary>
    public class GremlinQueryExecutionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="GremlinQueryExecutionException"/>.
        /// </summary>
        /// <param name="executionContext">The execution context of the failed query.</param>
        /// <param name="innerException">The exception that caused the failure.</param>
        public GremlinQueryExecutionException(GremlinQueryExecutionContext executionContext, Exception innerException) : this(executionContext, $"Executing query {executionContext.ExecutionId:D} failed.", innerException)
        {
            ArgumentNullException.ThrowIfNull(innerException);

        }

        /// <summary>
        /// Initializes a new instance of <see cref="GremlinQueryExecutionException"/> with a custom message.
        /// </summary>
        /// <param name="executionContext">The execution context of the failed query.</param>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The exception that caused the failure.</param>
        public GremlinQueryExecutionException(GremlinQueryExecutionContext executionContext, string message, Exception innerException) : base(message, innerException)
        {
            ArgumentNullException.ThrowIfNull(message);
            ArgumentNullException.ThrowIfNull(innerException);

            ExecutionContext = executionContext;
        }

        /// <summary>
        /// Gets the execution context of the failed query.
        /// </summary>
        public GremlinQueryExecutionContext ExecutionContext { get; }
    }
}
