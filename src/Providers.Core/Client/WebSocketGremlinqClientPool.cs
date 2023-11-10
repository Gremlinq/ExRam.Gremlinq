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
        private readonly IGremlinqClient?[] _clients;
        private readonly GremlinServer _server;
        private readonly ConnectionPoolSettings _poolSettings;
        private readonly Action<ClientWebSocketOptions> _optionsTransformation;
        private readonly IGremlinQueryEnvironment _environment;

        public WebSocketGremlinqClientPool(GremlinServer server, ConnectionPoolSettings poolSettings, Action<ClientWebSocketOptions> optionsTransformation, IGremlinQueryEnvironment environment)
        {
            _server = server;
            _poolSettings = poolSettings;
            _optionsTransformation = optionsTransformation;
            _environment = environment;

            _clients = new IGremlinqClient?[poolSettings.PoolSize];
        }

        public IAsyncEnumerable<ResponseMessage<T>> SubmitAsync<T>(RequestMessage message)
        {
            return Core(message, this);

            static async IAsyncEnumerable<ResponseMessage<T>> Core(RequestMessage message, WebSocketGremlinqClientPool @this, [EnumeratorCancellation] CancellationToken ct = default)
            {
                var slot = Math.Abs((Interlocked.Increment(ref @this._connectionIndex) - 1) % @this._clients.Length);

                while (true)
                {
                    var maybeClient = @this._clients[slot];

                    if (maybeClient is { } client)
                    {
                        await using (var e = client.SubmitAsync<T>(message).WithCancellation(ct).GetAsyncEnumerator())
                        {
                            while (true)
                            {
                                try
                                {
                                    if (!await e.MoveNextAsync())
                                        break;
                                }
                                catch (Exception)
                                {
                                    using (client)
                                    {
                                        Interlocked.CompareExchange(ref @this._clients[slot], null, client);
                                    }

                                    throw;
                                }

                                yield return e.Current;
                            }
                        }

                        yield break;
                    }
                    else
                        Interlocked.CompareExchange(ref @this._clients[slot], new WebSocketGremlinqClient(@this._server, @this._optionsTransformation, @this._environment), null);
                }
            }
        }

        public void Dispose()
        {
            foreach (var client in _clients)
            {
                client?.Dispose();
            }
        }
    }
}

