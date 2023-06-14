namespace ExRam.Gremlinq.Core.Execution
{
    public class GremlinQueryExecutionException : Exception
    {
        public GremlinQueryExecutionException(Guid requestId, Exception innerException) : base($"Executing query {requestId:N} failed.", innerException)
        {
            RequestId = requestId;
        }

        public Guid RequestId { get; }
    }
}
