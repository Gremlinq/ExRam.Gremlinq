using ExRam.Gremlinq.Core;
using System.Net.WebSockets;

using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;
using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Providers.Core
{
    internal sealed class WebSocketGremlinqClientPool : IGremlinqClient
    {
        private int _connectionIndex = 0;
        private readonly IGremlinqClient[] _clients;

        public WebSocketGremlinqClientPool(GremlinServer server, ConnectionPoolSettings poolSettings, Action<ClientWebSocketOptions> optionsTransformation, IGremlinQueryEnvironment environment)
        {
            _clients = Enumerable
                .Range(0, poolSettings.PoolSize)
                .Select(_ => new WebSocketGremlinqClient(server, optionsTransformation, environment))
                .ToArray();
        }

        public IAsyncEnumerable<ResponseMessage<T>> SubmitAsync<T>(RequestMessage message)
        {
            return Core(message, this);

            static async IAsyncEnumerable<ResponseMessage<T>> Core(RequestMessage message, WebSocketGremlinqClientPool @this, [EnumeratorCancellation] CancellationToken ct = default)
            {
                var slot = Math.Abs((Interlocked.Increment(ref @this._connectionIndex) - 1) % @this._clients.Length);

                try
                {
                    await foreach(var item in @this._clients[slot].SubmitAsync<T>(message).WithCancellation(ct))
                    {
                        yield return item;
                    }
                }
                finally
                {
                    Interlocked.Decrement(ref @this._connectionIndex);
                }
            }
        }

        public void Dispose()
        {
            foreach(var client in _clients)
            {
                client.Dispose();
            }
        }
    }
}

