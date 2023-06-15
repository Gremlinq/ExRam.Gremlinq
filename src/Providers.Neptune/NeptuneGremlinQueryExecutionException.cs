using ExRam.Gremlinq.Core.Execution;

namespace ExRam.Gremlinq.Providers.Neptune
{
    public sealed class NeptuneGremlinQueryExecutionException : GremlinQueryExecutionException
    {
        public NeptuneGremlinQueryExecutionException(NeptuneErrorCode code, GremlinQueryExecutionContext executionContext, string message, Exception innerException) : base(executionContext, message, innerException)
        {
            Code = code;
        }

        public NeptuneGremlinQueryExecutionException(NeptuneErrorCode code, GremlinQueryExecutionContext executionContext, Exception innerException) : base(executionContext, innerException)
        {
            Code = code;
        }

        public NeptuneErrorCode Code { get; }
    }
}
