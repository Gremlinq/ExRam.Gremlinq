using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.Core
{
    internal static class GremlinqClient
    {
        private sealed class DisposedGremlinqClient : IGremlinqClient
        {
            public void Dispose()
            {
            }

            public IAsyncEnumerable<ResponseMessage<T>> SubmitAsync<T>(RequestMessage message)
            {
                throw new ObjectDisposedException(nameof(DisposedGremlinqClient));
            }
        }

        public static readonly IGremlinqClient Disposed = new DisposedGremlinqClient();
    }
}
