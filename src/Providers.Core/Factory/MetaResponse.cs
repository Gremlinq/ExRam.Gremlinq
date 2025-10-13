using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.Core
{
    public readonly struct MetaResponse<T>
    {
        private readonly T[]? _values;
        private readonly ResponseStatus? _responseStatus;

        internal MetaResponse(T[] values, ResponseStatus responseStatus)
        {
            _values = values;
            _responseStatus = responseStatus;
        }

        public T[] Values => _values ?? throw new InvalidOperationException();

        public ResponseStatus ResponseStatus => _responseStatus ?? throw new InvalidOperationException();
    }
}
