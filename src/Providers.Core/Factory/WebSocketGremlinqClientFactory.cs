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
    public static class WebSocketGremlinqClientFactory
    {
        private sealed class WebSocketGremlinqClientFactoryImpl : IWebSocketGremlinqClientFactory
        {
            private sealed class WebSocketGremlinqClient : IGremlinqClient
            {
                private abstract class Channel
                {
                    protected Channel(Guid requestId, IGremlinQueryEnvironment environment)
                    {
                        RequestId = requestId;
                        Environment = environment;
                    }

                    public abstract void Signal(ReadOnlyMemory<byte> bytes);

                    public Guid RequestId { get; }
                    public IGremlinQueryEnvironment Environment { get; }
                }

                private sealed class Channel<T> : Channel, IAsyncEnumerable<ResponseMessage<T>>, IDisposable
                {
                    private sealed class MessageQueue
                    {
                        public void Signal(ResponseMessage<T> message)
                        {
                            Queue.Enqueue(message);
                            Semaphore.Release();
                        }

                        public SemaphoreSlim Semaphore { get; } = new(0);
                        public ConcurrentQueue<ResponseMessage<T>> Queue { get; } = new();
                    }

                    private readonly TaskCompletionSource<object> _tcs = new ();

                    public Channel(Guid requestId, IGremlinQueryEnvironment environment) : base(requestId, environment)
                    {
                    }

                    public override void Signal(ReadOnlyMemory<byte> bytes)
                    {
                        if (Environment.Deserializer.TryTransform(bytes, Environment, out ResponseMessage<T>? response))
                            Signal(response);
                        else
                            throw new InvalidOperationException();
                    }

                    private void Signal(ResponseMessage<T> response)
                    {
                        if (_tcs.Task.IsCompletedSuccessfully)
                        {
                            if (_tcs.Task.Result is MessageQueue messageQueue)
                                messageQueue.Signal(response);
                        }
                        else if (response.Status.Code != PartialContent)
                            _tcs.TrySetResult(response);
                        else if (_tcs.TrySetResult(new MessageQueue()))
                            Signal(response);
                    }

                    public async IAsyncEnumerator<ResponseMessage<T>> GetAsyncEnumerator(CancellationToken ct = default)
                    {
                        var obj = await _tcs.Task;

                        if (obj is ResponseMessage<T> reponseMessage)
                            yield return reponseMessage;
                        else if (obj is MessageQueue { Queue: var queue, Semaphore: var semaphore})
                        {
                            while (true)
                            {
                                await semaphore.WaitAsync(ct);

                                if (queue.TryDequeue(out var response))
                                {
                                    yield return response;

                                    if (response.Status.Code != PartialContent)
                                        break;
                                }
                            }
                        }
                        else
                            throw new InvalidOperationException();
                    }

                    public void Dispose()
                    {
                        if (_tcs.Task.IsCompletedSuccessfully)
                        {
                            if (_tcs.Task.Result is MessageQueue { Semaphore: var semaphore })
                                semaphore.Dispose();
                        }
                        else if (!_tcs.Task.IsFaulted)
                        {
                            if (!_tcs.TrySetException(new ObjectDisposedException(nameof(Channel<T>))))
                                Dispose();
                        }
                    }
                }

                private record struct ResponseStatus(ResponseStatusCode Code);

                private record struct ResponseMessageEnvelope(Guid? RequestId, ResponseStatus? Status);


                private readonly GremlinServer _server;
                private readonly ClientWebSocket _client = new();
                private readonly SemaphoreSlim _sendLock = new(1);
                private readonly CancellationTokenSource _cts = new();
                private readonly IGremlinQueryEnvironment _environment;
                private readonly TaskCompletionSource<Task> _loopTcs = new();
                private readonly ConcurrentDictionary<Guid, Channel> _channels = new();

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
                        if (@this._loopTcs.Task is { IsCompleted: true } loopTask)
                            await loopTask;

                        using (var channel = new Channel<T>(message.RequestId, @this._environment))
                        {
                            using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, @this._cts.Token))
                            {
                                @this._channels.TryAdd(message.RequestId, channel);

                                try
                                {
                                    await @this.SendCore(message, linkedCts.Token);

                                    await foreach (var item in channel.WithCancellation(linkedCts.Token))
                                    {
                                        yield return item;
                                    }
                                }
                                finally
                                {
                                    @this._channels.TryRemove(message.RequestId, out _);
                                }
                            }
                        }
                    }
                }

                public void Dispose()
                {
                    using (_sendLock)
                    {
                        using (_client)
                        {
                            using (_cts)
                            {
                                _cts.Cancel();
                                _loopTcs.TrySetException(new ObjectDisposedException(nameof(WebSocketGremlinqClient)));
                            }
                        }
                    }
                }

                private async Task SendCore(RequestMessage requestMessage, CancellationToken ct)
                {
                    await _sendLock.WaitAsync(ct);

                    try
                    {
                        try
                        {
                            if (_client.State == WebSocketState.None)
                            {
                                await _client.ConnectAsync(_server.Uri, ct);

                                _loopTcs.SetResult(Loop(_cts.Token));
                            }

                            if (_environment.Serializer.TryTransform(requestMessage, _environment, out byte[]? serializedRequest))
                                await _client.SendAsync(serializedRequest, WebSocketMessageType.Binary, true, ct);
                        }
                        finally
                        {
                            _sendLock.Release();

                        }
                    }
                    catch
                    {
                        Dispose();

                        throw;
                    }
                }

                private async Task Loop(CancellationToken ct)
                {
                    using (this)
                    {
                        while (!ct.IsCancellationRequested)
                        {
                            var bytes = await _client.ReceiveAsync(ct);

                            using (bytes)
                            {
                                if (_environment.Deserializer.TryTransform(bytes.Memory, _environment, out ResponseMessageEnvelope responseMessageEnvelope))
                                {
                                    if (responseMessageEnvelope is { Status.Code: var statusCode, RequestId: { } requestId })
                                    {
                                        if (statusCode == Authenticate)
                                        {
                                            var authMessage = RequestMessage
                                                .Build(Tokens.OpsAuthentication)
                                                .Processor(Tokens.ProcessorTraversal)
                                                .AddArgument(Tokens.ArgsSasl, Convert.ToBase64String(Encoding.UTF8.GetBytes($"\0{_server.Username}\0{_server.Password}")))
                                                .Create();

                                            await SendCore(authMessage, ct);
                                        }
                                        else
                                        {
                                            if (_channels.TryGetValue(requestId, out var otherChannel))
                                                otherChannel.Signal(bytes.Memory);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            public static readonly WebSocketGremlinqClientFactoryImpl LocalHost = new(new GremlinServer(), _ => { });

            private readonly GremlinServer _server;
            private readonly Action<ClientWebSocketOptions> _webSocketOptionsConfiguration;

            private WebSocketGremlinqClientFactoryImpl(GremlinServer server, Action<ClientWebSocketOptions> webSocketOptionsConfiguration)
            {
                if (!"ws".Equals(server.Uri.Scheme, StringComparison.OrdinalIgnoreCase) && !"wss".Equals(server.Uri.Scheme, StringComparison.OrdinalIgnoreCase))
                    throw new ArgumentException("Expected the Uri-Scheme to be either \"ws\" or \"wss\".");

                _server = server;
                _webSocketOptionsConfiguration = webSocketOptionsConfiguration;
            }

            public IWebSocketGremlinqClientFactory ConfigureServer(Func<GremlinServer, GremlinServer> transformation) => new WebSocketGremlinqClientFactoryImpl(transformation(_server), _webSocketOptionsConfiguration);

            public IWebSocketGremlinqClientFactory ConfigureOptions(Action<ClientWebSocketOptions> configuration) => new WebSocketGremlinqClientFactoryImpl(
                _server,
                options =>
                {
                    _webSocketOptionsConfiguration(options);
                    configuration(options);
                });

            public IGremlinqClient Create(IGremlinQueryEnvironment environment) => new WebSocketGremlinqClient(_server, _webSocketOptionsConfiguration, environment);
        }

        public static readonly IWebSocketGremlinqClientFactory LocalHost = WebSocketGremlinqClientFactoryImpl.LocalHost;
    }
}
