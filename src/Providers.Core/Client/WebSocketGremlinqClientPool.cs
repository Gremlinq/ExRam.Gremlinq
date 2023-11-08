using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.Core
{
    internal sealed class WebSocketGremlinqClientPool : IGremlinClient
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<ResultSet<T>> SubmitAsync<T>(RequestMessage requestMessage, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}

