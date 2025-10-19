using Gremlin.Net.Driver.Messages;

using static ExRam.Gremlinq.Core.ExceptionHelper;

namespace ExRam.Gremlinq.Providers.Core
{
    public readonly struct MetaResponse<T>
    {
        private readonly T[]? _data;
        private readonly Guid _requestId;
        private readonly ResponseStatus? _responseStatus;

        internal MetaResponse(Guid requestId, T[]? data, ResponseStatus responseStatus)
        {
            _data = data;
            _requestId = requestId;
            _responseStatus = responseStatus;
        }

        public Guid RequestId => _responseStatus is not null
            ? _requestId
            : throw UninitializedStruct();

        public T[]? Data => _responseStatus is not null
            ? _data
            : throw UninitializedStruct();

        public ResponseStatus ResponseStatus => _responseStatus ?? throw UninitializedStruct();
    }
}
