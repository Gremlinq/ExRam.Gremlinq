using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.Core
{
    public readonly struct MetaResponse<T>
    {
        private readonly T _value;
        private readonly ResponseStatus? _responseStatus;

        internal MetaResponse(T value, ResponseStatus responseStatus)
        {
            _value = value;
            _responseStatus = responseStatus;
        }

        public T Value => _responseStatus is not null
            ? _value
            : throw new InvalidOperationException();

        public ResponseStatus ResponseStatus => _responseStatus ?? throw new InvalidOperationException();
    }
}
