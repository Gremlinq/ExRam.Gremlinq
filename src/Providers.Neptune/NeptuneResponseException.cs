namespace ExRam.Gremlinq.Providers.Neptune
{
    public sealed class NeptuneResponseException : Exception
    {
        public NeptuneResponseException(NeptuneErrorCode code, string detailedMessage, Exception innerException) : base(detailedMessage, innerException)
        {
            Code = code;
        }

        public NeptuneErrorCode Code { get; }
    }
}
