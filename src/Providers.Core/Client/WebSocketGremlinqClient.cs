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

        private sealed class Channel<T> : IChannel, IAsyncEnumerable<ResponseMessage<T>>, IDisposable
        {
            private SemaphoreSlim? _semaphore;
            private ConcurrentQueue<object>? _queue;

            private readonly IGremlinQueryEnvironment _environment;

            public Channel(Guid requestId, IGremlinQueryEnvironment environment)
            {
                RequestId = requestId;
                _environment = environment;
            }

            public void Signal(ReadOnlyMemory<byte> bytes)
            {
                var (semaphore, queue) = GetTuple();

                try
                {
                    if (_environment.Deserializer.TryTransform(bytes, _environment, out ResponseMessage<T>? response))
                    {
                        queue.Enqueue(response);
                        semaphore.Release();
                    }
                }
                catch (Exception ex)
                {
                    queue.Enqueue(ex);
                    semaphore.Release();
                }
            }

            public async IAsyncEnumerator<ResponseMessage<T>> GetAsyncEnumerator(CancellationToken ct = default)
            {
                var (semaphore, queue) = GetTuple();

                while (true)
                {
                    await semaphore.WaitAsync(ct);

                    if (queue.TryDequeue(out var item))
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

            private (SemaphoreSlim, ConcurrentQueue<object>) GetTuple()
            {
                if (_semaphore is { } semaphore)
                {
                    if (_queue is { } queue)
                        return (semaphore, queue);

                    Interlocked.CompareExchange(ref _queue, new ConcurrentQueue<object>(), null);
                    return GetTuple();
                }
                else
                {
                    var newSemaphore = new SemaphoreSlim(0);
                    if (Interlocked.CompareExchange(ref _semaphore, newSemaphore, null) != null)
                        newSemaphore.Dispose();
                }

                return GetTuple();
            }

            public void Dispose()
            {
                _semaphore?.Dispose();
            }

            public Guid RequestId { get; }
        }

        private record struct ResponseStatus(ResponseStatusCode Code);

        private record struct ResponseMessageEnvelope(Guid? RequestId, ResponseStatus? Status);

        private readonly GremlinServer _server;
        private readonly ClientWebSocket _client = new();
        private readonly SemaphoreSlim _sendLock = new(1);
        private readonly SemaphoreSlim _receiveLock = new(1);
        private readonly CancellationTokenSource _cts = new();
        private readonly IGremlinQueryEnvironment _environment;
        private readonly ConcurrentDictionary<Guid, IChannel> _channels = new();

        public WebSocketGremlinqClient(GremlinServer server, Action<ClientWebSocketOptions> optionsTransformation, IGremlinQueryEnvironment environment)
        {
            _server = server;
            _environment = environment;

            _client.Options.SetRequestHeader("User-Agent", "ExRam.Gremlinq");

            optionsTransformation(_client.Options);
        }

        public IAsyncEnumerable<ResponseMessage<T>> SubmitAsync<T>(RequestMessage message)
        {
            return Core(message, this);

            static async IAsyncEnumerable<ResponseMessage<T>> Core(RequestMessage message, WebSocketGremlinqClient @this, [EnumeratorCancellation] CancellationToken ct = default)
            {
                using (var channel = new Channel<T>(message.RequestId, @this._environment))
                {
                    @this._channels.TryAdd(message.RequestId, channel);

                    await @this.SendCore(message, ct);

                    await @this._receiveLock.WaitAsync(ct);

                    try
                    {
                        while (true)
                        {
                            var bytes = await @this._client.ReceiveAsync(ct);

                            using (bytes)
                            {
                                if (@this._environment.Deserializer.TryTransform(bytes.Memory, @this._environment, out ResponseMessageEnvelope responseMessageEnvelope))
                                {
                                    if (responseMessageEnvelope is { Status.Code: var statusCode, RequestId: { } requestId })
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
                                        else if (channel.RequestId == requestId)
                                        {
                                            if (@this._environment.Deserializer.TryTransform(bytes.Memory, @this._environment, out ResponseMessage<T>? response))
                                                yield return response;

                                            if (statusCode != PartialContent)
                                            {
                                                @this._channels.TryRemove(requestId, out _);

                                                yield break;
                                            }
                                        }
                                        else if (statusCode == PartialContent)
                                        {
                                            if (@this._channels.TryGetValue(requestId, out var otherChannel))
                                                otherChannel.Signal(bytes.Memory);
                                        }
                                        else
                                        {
                                            if (@this._channels.TryRemove(requestId, out var otherChannel))
                                                otherChannel.Signal(bytes.Memory);

                                            break;
                                        }
                                    }
                                }
                                else
                                    throw new InvalidOperationException();
                            }
                        }
                    }
                    finally
                    {
                        @this._receiveLock.Release();
                    }

                    await foreach (var response in channel.WithCancellation(CancellationTokenSource.CreateLinkedTokenSource(ct, @this._cts.Token).Token))
                    {
                        yield return response;
                    }
                }
            }
        }

        public void Dispose()
        {
            using (_receiveLock)
            {
                using (_sendLock)
                {
                    using (_client)
                    {
                        _cts.Cancel();
                    }
                }
            }
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
    }
}

