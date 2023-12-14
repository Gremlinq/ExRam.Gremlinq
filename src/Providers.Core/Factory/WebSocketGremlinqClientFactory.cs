using System.Buffers;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;

using CommunityToolkit.HighPerformance.Buffers;

using ExRam.Gremlinq.Core;

using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;

using static Gremlin.Net.Driver.Messages.ResponseStatusCode;

namespace ExRam.Gremlinq.Providers.Core
{
    public static class WebSocketGremlinqClientFactory
    {
        private sealed class WebSocketGremlinqClientFactoryImpl<TBuffer> : IWebSocketGremlinqClientFactory
            where TBuffer : IMemoryOwner<byte>
        {
            private sealed class WebSocketGremlinqClient : IGremlinqClient
            {
                private abstract class Channel
                {
                    protected Channel(IGremlinQueryEnvironment environment)
                    {
                        Environment = environment;
                    }

                    public abstract void Signal(TBuffer buffer);

                    protected IGremlinQueryEnvironment Environment { get; }
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

                    public Channel(IGremlinQueryEnvironment environment) : base(environment)
                    {
                    }

                    public override void Signal(TBuffer buffer)
                    {
                        try
                        {
                            if (Environment.Deserializer.TryTransform(buffer, Environment, out ResponseMessage<T>? response))
                                Signal(response);
                            else
                                throw new InvalidOperationException($"Unable to convert byte array to a {nameof(ResponseMessage<T>)} for {typeof(T).FullName}>.");
                        }
                        catch
                        {
                            Dispose();

                            throw;
                        }
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

                        if (obj is ResponseMessage<T> responseMessage)
                            yield return responseMessage;
                        else if (obj is MessageQueue { Queue: var queue, Semaphore: var semaphore })
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
                            throw new NotSupportedException();
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

                private readonly byte[] _mimeTypeBytes;
                private readonly ClientWebSocket _client = new();
                private readonly SemaphoreSlim _sendLock = new(1);
                private readonly CancellationTokenSource _cts = new();
                private readonly IGremlinQueryEnvironment _environment;
                private readonly TaskCompletionSource<Task> _loopTcs = new();
                private readonly WebSocketGremlinqClientFactoryImpl<TBuffer> _factory;
                private readonly ConcurrentDictionary<Guid, Channel> _channels = new();

                public WebSocketGremlinqClient(WebSocketGremlinqClientFactoryImpl<TBuffer> factory, IGremlinQueryEnvironment environment, Action<ClientWebSocketOptions> optionsTransformation)
                {
                    _factory = factory;
                    _environment = environment;
                    _client.Options.SetRequestHeader("User-Agent", "ExRam.Gremlinq");
                    _mimeTypeBytes = Encoding.UTF8.GetBytes($"{(char)factory._bufferFactory.MimeType.Length}{factory._bufferFactory.MimeType}");

                    optionsTransformation(_client.Options);
                }

                public IAsyncEnumerable<ResponseMessage<T>> SubmitAsync<T>(RequestMessage message)
                {
                    return Core(message, this);

                    static async IAsyncEnumerable<ResponseMessage<T>> Core(RequestMessage message, WebSocketGremlinqClient @this, [EnumeratorCancellation] CancellationToken ct = default)
                    {
                        if (@this._loopTcs.Task is { IsCompleted: true } loopTask)
                            await loopTask;

                        using (var channel = new Channel<T>(@this._environment))
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
                            _cts.Cancel();
                            _loopTcs.TrySetException(new ObjectDisposedException(nameof(WebSocketGremlinqClient)));
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
                                await _client.ConnectAsync(_factory._uri, ct);

                                _loopTcs.SetResult(Loop(_cts.Token));
                            }

                            using (var serializedRequest = _factory._bufferFactory.Create(requestMessage))
                            {
                                using (var buffer = MemoryOwner<byte>.Allocate(serializedRequest.Memory.Length + _mimeTypeBytes.Length))
                                {
                                    _mimeTypeBytes.CopyTo(buffer.Span);
                                    serializedRequest.Memory.Span.CopyTo(buffer.Span[_mimeTypeBytes.Length..]);

                                    await _client.SendAsync(buffer.Memory, WebSocketMessageType.Binary, true, ct);
                                }
                            }
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

                            using (var buffer = _factory._bufferFactory.Create(bytes))
                            {
                                if (_environment.Deserializer.TryTransform(buffer, _environment, out ResponseMessageEnvelope responseMessageEnvelope))
                                {
                                    if (responseMessageEnvelope is { Status.Code: var statusCode, RequestId: { } requestId })
                                    {
                                        if (statusCode == Authenticate)
                                        {
                                            if (_factory._username is { Length: > 0 } username && _factory._password is { Length: > 0 } password)
                                            {
                                                var authMessage = RequestMessage
                                                    .Build(Tokens.OpsAuthentication)
                                                    .Processor(Tokens.ProcessorTraversal)
                                                    .AddArgument(Tokens.ArgsSasl, Convert.ToBase64String(Encoding.UTF8.GetBytes($"\0{username}\0{password}")))
                                                    .Create();

                                                await SendCore(authMessage, ct);
                                            }
                                            else
                                                throw new NotSupportedException("Authentication credentials were requested from the server but were not configured.");
                                        }
                                        else
                                        {
                                            if (_channels.TryGetValue(requestId, out var otherChannel))
                                                otherChannel.Signal(buffer);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            private readonly Uri _uri;
            private readonly string? _username;
            private readonly string? _password;
            private readonly IMessageBufferFactory<TBuffer> _bufferFactory;
            private readonly Action<ClientWebSocketOptions> _webSocketOptionsConfiguration;

            internal WebSocketGremlinqClientFactoryImpl(Uri uri, string? username, string? password, Action<ClientWebSocketOptions> webSocketOptionsConfiguration, IMessageBufferFactory<TBuffer> bufferFactory)
            {
                if (uri.Scheme is not "ws" and not "wss")
                    throw new ArgumentException($"Expected {nameof(uri)}.{nameof(Uri.Scheme)} to be either \"ws\" or \"wss\".", nameof(uri));

                _username = username;
                _password = password;
                _uri = uri.EnsurePath();
                _bufferFactory = bufferFactory;
                _webSocketOptionsConfiguration = webSocketOptionsConfiguration;
            }

            public IWebSocketGremlinqClientFactory ConfigureOptions(Action<ClientWebSocketOptions> configuration) => new WebSocketGremlinqClientFactoryImpl<TBuffer>(
                _uri,
                _username,
                _password,
                options =>
                {
                    _webSocketOptionsConfiguration(options);
                    configuration(options);
                },
                _bufferFactory);

            public IGremlinqClient Create(IGremlinQueryEnvironment environment) => new WebSocketGremlinqClient(this, environment, _webSocketOptionsConfiguration);

            public IWebSocketGremlinqClientFactory ConfigureUri(Func<Uri, Uri> transformation) => new WebSocketGremlinqClientFactoryImpl<TBuffer>(transformation(_uri), _username, _password, _webSocketOptionsConfiguration, _bufferFactory);

            public IWebSocketGremlinqClientFactory ConfigureUsername(Func<string?, string?> transformation) => new WebSocketGremlinqClientFactoryImpl<TBuffer>(_uri, transformation(_username), _password, _webSocketOptionsConfiguration, _bufferFactory);

            public IWebSocketGremlinqClientFactory ConfigurePassword(Func<string?, string?> transformation) => new WebSocketGremlinqClientFactoryImpl<TBuffer>(_uri, _username, transformation(_password), _webSocketOptionsConfiguration, _bufferFactory);

            public IWebSocketGremlinqClientFactory WithMessageBufferFactory<TNewBuffer>(IMessageBufferFactory<TNewBuffer> factory) where TNewBuffer : IMemoryOwner<byte> => new WebSocketGremlinqClientFactoryImpl<TNewBuffer>(_uri, _username, _password, _webSocketOptionsConfiguration, factory);
        }

        public static readonly IWebSocketGremlinqClientFactory LocalHost = new WebSocketGremlinqClientFactoryImpl<GraphSon3MessageBuffer>(new Uri("ws://localhost:8182"), null, null, _ => { }, MessageBufferFactory.GraphSon3);
    }
}
