using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.Core
{
    public readonly struct MetaResponse<T>
    {
        private readonly T[]? _data;
        private readonly ResponseStatus? _responseStatus;

        internal MetaResponse(T[] data, ResponseStatus responseStatus)
        {
            _data = data;
            _responseStatus = responseStatus;
        }

        public T[] Data => _data ?? throw new InvalidOperationException();

        public ResponseStatus ResponseStatus => _responseStatus ?? throw new InvalidOperationException();
    }
}
