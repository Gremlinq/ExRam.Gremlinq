using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;

using ExRam.Gremlinq.Core;

using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;

using static Gremlin.Net.Driver.Messages.ResponseStatusCode;

namespace ExRam.Gremlinq.Providers.Core
{
    internal sealed class WebSocketGremlinqClient : IGremlinqClient
    {
        private interface IChannel
        {
            void Signal(ReadOnlyMemory<byte> bytes);
        }

        private sealed class Channel<T> : IChannel, IAsyncEnumerable<ResponseMessage<T>>
        {
            private readonly SemaphoreSlim _semaphore = new(0);
            private readonly IGremlinQueryEnvironment _environment;
            private readonly ConcurrentQueue<object> _queue = new ();

            public Channel(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            public void Signal(ReadOnlyMemory<byte> bytes)
            {
                try
                {
                    if (_environment.Deserializer.TryTransform(bytes, _environment, out ResponseMessage<T>? response))
                    {
                        _queue.Enqueue(response);
                        _semaphore.Release();
                    }
                }
                catch (Exception ex)
                {
                    _queue.Enqueue(ex);
                    _semaphore.Release();
                }
            }

            public async IAsyncEnumerator<ResponseMessage<T>> GetAsyncEnumerator(CancellationToken ct = default)
            {
                while (true)
                {
                    await _semaphore.WaitAsync(ct);

                    if (_queue.TryDequeue(out var item))
                    {
                        if (item is ResponseMessage<T> response)
                        {
                            yield return response;

                            if (response.Status.Code != PartialContent)
                                break;
                        }
                        else if (item is Exception ex)
                            throw ex;
                    }
                }
            }
        }

        private record struct ResponseMessageEnvelope(Guid? RequestId, ResponseStatus? Status);

        private record struct ResponseStatus(ResponseStatusCode Code);

        private readonly GremlinServer _server;
        private readonly ClientWebSocket _client = new();
        private readonly SemaphoreSlim _sendLock = new(1);
        private readonly SemaphoreSlim _receiveLock = new(1);
        private readonly IGremlinQueryEnvironment _environment;
        private readonly ConcurrentDictionary<Guid, IChannel> _states = new();

        public WebSocketGremlinqClient(GremlinServer server, IGremlinQueryEnvironment environment)
        {
            _server = server;
            _environment = environment;

            _client.Options.SetRequestHeader("User-Agent", "ExRam.Gremlinq");
        }

        public IAsyncEnumerable<ResponseMessage<T>> SubmitAsync<T>(RequestMessage message)
        {
            return Core(message, this);

            static async IAsyncEnumerable<ResponseMessage<T>> Core(RequestMessage message, WebSocketGremlinqClient @this, [EnumeratorCancellation] CancellationToken ct = default)
            {
                var maybeException = default(Exception?);
                var state = new Channel<T>(@this._environment);

                @this._states.TryAdd(message.RequestId, state);

                var loopTask = Loop(message, @this, ct);

                try
                {
                    await foreach (var response in state)
                    {
                        yield return response;
                    }
                }
                finally
                {
                    try
                    {
                        await loopTask;
                    }
                    catch (Exception ex)
                    {
                        maybeException = ex;
                    }
                }

                if (maybeException is { } exception)
                {
                    using (@this)
                    {
                        throw new ObjectDisposedException(nameof(WebSocketGremlinqClient), exception);
                    }
                }
            }

            static async Task Loop(RequestMessage message, WebSocketGremlinqClient @this, CancellationToken ct)
            {
                await @this.SendCore(message, ct);

                while (true)
                {
                    var (envelope, bytes) = await @this.ReceiveCore(ct);

                    using (bytes)
                    {
                        if (envelope is { Status.Code: var statusCode, RequestId: { } requestId })
                        {
                            if (statusCode == Authenticate)
                            {
                                var authMessage = RequestMessage
                                    .Build(Tokens.OpsAuthentication)
                                    .Processor(Tokens.ProcessorTraversal)
                                    .AddArgument(Tokens.ArgsSasl, Convert.ToBase64String(Encoding.UTF8.GetBytes($"\0{@this._server.Username}\0{@this._server.Password}")))
                                    .Create();

                                await @this.SendCore(authMessage, ct);
                            }
                            else if (statusCode == PartialContent)
                            {
                                if (@this._states.TryGetValue(requestId, out var state))
                                    state.Signal(bytes.Memory);
                            }
                            else
                            {
                                if (@this._states.TryRemove(requestId, out var state))
                                    state.Signal(bytes.Memory);

                                break;
                            }
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        private async Task SendCore(RequestMessage requestMessage, CancellationToken ct)
        {
            await _sendLock.WaitAsync(ct);

            try
            {
                if (_client.State == WebSocketState.None)
                    await _client.ConnectAsync(_server.Uri, ct);

                if (_environment.Serializer.TryTransform(requestMessage, _environment, out byte[]? serializedRequest))
                    await _client.SendAsync(serializedRequest, WebSocketMessageType.Binary, true, ct);
            }
            finally
            {
                _sendLock.Release();
            }
        }

        private async Task<(ResponseMessageEnvelope Envelope, ClientWebSocketExtensions.SlicedMemoryOwner Bytes)> ReceiveCore(CancellationToken ct)
        {
            await _receiveLock.WaitAsync(ct);

            try
            {
                var bytes = await _client.ReceiveAsync(ct);

                if (_environment.Deserializer.TryTransform(bytes.Memory, _environment, out ResponseMessageEnvelope responseMessageEnvelope))
                    return (responseMessageEnvelope, bytes);

                using (bytes)
                {
                    throw new InvalidOperationException();
                }
            }
            finally
            {
                _receiveLock.Release();
            }
        }
    }
}

