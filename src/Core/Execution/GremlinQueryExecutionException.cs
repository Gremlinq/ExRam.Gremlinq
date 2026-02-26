namespace ExRam.Gremlinq.Core.Execution
{
    public class GremlinQueryExecutionException : Exception
    {
        public GremlinQueryExecutionException(GremlinQueryExecutionContext executionContext, Exception innerException) : this(executionContext, $"Executing query {executionContext.ExecutionId:D} failed.", innerException)
        {
            ArgumentNullException.ThrowIfNull(innerException);

        }

        public GremlinQueryExecutionException(GremlinQueryExecutionContext executionContext, string message, Exception innerException) : base(message, innerException)
        {
            ArgumentNullException.ThrowIfNull(message);
            ArgumentNullException.ThrowIfNull(innerException);

            ExecutionContext = executionContext;
        }

        public GremlinQueryExecutionContext ExecutionContext { get; }
    }
}
