using ExRam.Gremlinq.Core.Execution;

namespace ExRam.Gremlinq.Providers.Neptune
{
    public sealed class NeptuneResponseException : GremlinQueryExecutionException
    {
        public NeptuneResponseException(NeptuneErrorCode code, string detailedMessage, Guid requestId, Exception innerException) : base(requestId, innerException)
        {
            Code = code;
            DetailedMessage = detailedMessage;
        }

        public NeptuneErrorCode Code { get; }

        public string DetailedMessage { get; }
    }
}
