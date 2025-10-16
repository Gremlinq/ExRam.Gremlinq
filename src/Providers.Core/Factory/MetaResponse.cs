using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.Core
{
    public readonly struct MetaResponse<T>
    {
        private readonly T[]? _data;
        private readonly Guid _requestId;
        private readonly ResponseStatus? _responseStatus;

        internal MetaResponse(Guid requestId, T[] data, ResponseStatus responseStatus)
        {
            _data = data;
            _requestId = requestId;
            _responseStatus = responseStatus;
        }

        public Guid RequestId => _responseStatus is not null
            ? _requestId
            : throw new NotImplementedException();

        public T[] Data => _data ?? throw new InvalidOperationException();

        public ResponseStatus ResponseStatus => _responseStatus ?? throw new InvalidOperationException();
    }
}
