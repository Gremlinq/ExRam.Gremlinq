namespace ExRam.Gremlinq.Core.Execution
{
    public class GremlinQueryExecutionException : Exception
    {
        public GremlinQueryExecutionException(GremlinQueryExecutionContext executionContext, Exception innerException) : this(executionContext, $"Executing query {executionContext.ExecutionId:N} failed.", innerException)
        {
        }

        public GremlinQueryExecutionException(GremlinQueryExecutionContext executionContext, string message, Exception innerException) : base(message, innerException)
        {
            ExecutionContext = executionContext;
        }

        public GremlinQueryExecutionContext ExecutionContext { get; }
    }
}
