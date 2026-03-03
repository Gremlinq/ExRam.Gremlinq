using Gremlin.Net.Driver.Messages;

using static ExRam.Gremlinq.Core.ExceptionHelper;

namespace ExRam.Gremlinq.Providers.Core
{
    /// <summary>
    /// Wraps a Gremlin response together with its request id and status information.
    /// </summary>
    /// <typeparam name="T">The type of the response data elements.</typeparam>
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

        /// <summary>
        /// Gets the request id that was assigned to this response.
        /// </summary>
        public Guid RequestId => _responseStatus is not null
            ? _requestId
            : throw UninitializedStruct();

        /// <summary>
        /// Gets the response data, or <c>null</c> if the response contained no data.
        /// </summary>
        public T[]? Data => _responseStatus is not null
            ? _data
            : throw UninitializedStruct();

        /// <summary>
        /// Gets the response status including status code and attributes.
        /// </summary>
        public ResponseStatus ResponseStatus => _responseStatus ?? throw UninitializedStruct();
    }
}
