using System.Buffers;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Net.WebSockets;
using System.Reflection;
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
                private abstract class Channel : IDisposable
                {
                    protected Channel(IGremlinQueryEnvironment environment)
                    {
                        Environment = environment;
                    }

                    public abstract void Signal(TBuffer buffer);

                    public abstract void Dispose();

                    protected IGremlinQueryEnvironment Environment { get; }
                }

                private sealed class Channel<T> : Channel, IAsyncEnumerable<ResponseMessage<T>>
                {
                    private readonly struct ResponseAndQueueUnion
                    {
                        private readonly SemaphoreSlim? _semaphore;
                        private readonly ResponseMessage<T>? _response;
                        private readonly ConcurrentQueue<ResponseMessage<T>>? _queue;

                        private ResponseAndQueueUnion(SemaphoreSlim semaphore, ConcurrentQueue<ResponseMessage<T>> queue)
                        {
                            _queue = queue;
                            _semaphore = semaphore;
                        }

                        private ResponseAndQueueUnion(ResponseMessage<T> response)
                        {
                            _response = response;
                        }

                        public bool TryGetResponse([NotNullWhen(true)] out ResponseMessage<T>? response) => (response = _response) is not null;

                        public bool TryGetQueue([NotNullWhen(true)] out SemaphoreSlim? semaphore, [NotNullWhen(true)] out ConcurrentQueue<ResponseMessage<T>>? queue)
                        {
                            if (_queue is { } actualQueue && _semaphore is { } actualSemphore)
                            {
                                queue = actualQueue;
                                semaphore = actualSemphore;

                                return true;
                            }

                            queue = null;
                            semaphore = null;

                            return false;
                        }

                        public static ResponseAndQueueUnion From(ResponseMessage<T> response) => new(response);

                        public static ResponseAndQueueUnion CreateQueue() => new(new (0), new());
                    }

                    private readonly TaskCompletionSource<ResponseAndQueueUnion> _tcs = new ();

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
                        while (true)
                        {
                            if (_tcs.Task.IsCompletedSuccessfully)
                            {
                                if (_tcs.Task.Result.TryGetQueue(out var semaphore, out var queue))
                                {
                                    queue.Enqueue(response);
                                    semaphore.Release();
                                }

                                return;
                            }
                            else
                            {
                                if (response.Status.Code != PartialContent)
                                {
                                    if (_tcs.TrySetResult(ResponseAndQueueUnion.From(response)))
                                        return;
                                }
                                else
                                    _tcs.TrySetResult(ResponseAndQueueUnion.CreateQueue());
                            }
                        }
                    }

                    public async IAsyncEnumerator<ResponseMessage<T>> GetAsyncEnumerator(CancellationToken ct = default)
                    {
                        var union = await _tcs.Task;

                        if (union.TryGetResponse(out var response))
                            yield return response;
                        else if (union.TryGetQueue(out var semaphore, out var queue))
                        {
                            while (true)
                            {
                                await semaphore.WaitAsync(ct);

                                if (queue.TryDequeue(out var queuedResponse))
                                {
                                    yield return queuedResponse;

                                    if (queuedResponse.Status.Code != PartialContent)
                                        break;
                                }
                            }
                        }
                        else
                            throw new NotSupportedException();
                    }

                    public override void Dispose()
                    {
                        while (true)
                        {
                            if (_tcs.Task.IsCompletedSuccessfully)
                            {
                                if (_tcs.Task.Result.TryGetQueue(out var semaphore, out _))
                                    semaphore.Dispose();

                                return;
                            }
                            else if (_tcs.Task.IsFaulted)
                                return;

                            if (_tcs.TrySetException(new ObjectDisposedException(nameof(Channel<T>))))
                                return;
                        }
                    }
                }

                private record struct ResponseStatus(ResponseStatusCode Code);

                private record struct ResponseMessageEnvelope(Guid? RequestId, ResponseStatus? Status);

                private readonly ClientWebSocket _client;
                private readonly SemaphoreSlim _sendLock = new(1);
                private readonly CancellationTokenSource _cts = new();
                private readonly IGremlinQueryEnvironment _environment;
                private readonly TaskCompletionSource<Task> _loopTcs = new();
                private readonly WebSocketGremlinqClientFactoryImpl<TBuffer> _factory;
                private readonly ConcurrentDictionary<Guid, Channel> _channels = new();

                public WebSocketGremlinqClient(WebSocketGremlinqClientFactoryImpl<TBuffer> factory, ClientWebSocket client, IGremlinQueryEnvironment environment)
                {
                    _client = client;
                    _factory = factory;
                    _environment = environment;
                }

                public IAsyncEnumerable<ResponseMessage<T>> SubmitAsync<T>(RequestMessage message)
                {
                    return Core(message, this);

                    static async IAsyncEnumerable<ResponseMessage<T>> Core(RequestMessage message, WebSocketGremlinqClient @this, [EnumeratorCancellation] CancellationToken ct = default)
                    {
                        using (var channel = new Channel<T>(@this._environment))
                        {
                            using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, @this._cts.Token))
                            {
                                @this._channels.TryAdd(message.RequestId, channel);

                                try
                                {
                                    await @this.SendCore(message, linkedCts.Token);

                                    await using (var e = channel.GetAsyncEnumerator(linkedCts.Token))
                                    {
                                        while (true)
                                        {
                                            try
                                            {
                                                if (!await e.MoveNextAsync())
                                                    break;
                                            }
                                            catch (ObjectDisposedException)
                                            {
                                                await await @this._loopTcs.Task;

                                                throw;
                                            }
                                            catch (OperationCanceledException)
                                            {
                                                @this.Dispose();

                                                await await @this._loopTcs.Task;

                                                throw;
                                            }

                                            yield return e.Current;
                                        }
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
                                var mimeTypeBytes = _factory._mimeTypeBytes ??= Encoding.UTF8.GetBytes($"{(char)_factory._bufferFactory.MimeType.Length}{_factory._bufferFactory.MimeType}");

                                using (var buffer = MemoryOwner<byte>.Allocate(serializedRequest.Memory.Length + mimeTypeBytes.Length))
                                {
                                    mimeTypeBytes.CopyTo(buffer.Span);
                                    serializedRequest.Memory.Span.CopyTo(buffer.Span[mimeTypeBytes.Length..]);

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
                                        if (_channels.TryGetValue(requestId, out var otherChannel))
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
                                                {
                                                    using (otherChannel)
                                                    {
                                                        throw new NotSupportedException("Authentication credentials were requested from the server but were not configured.");
                                                    }
                                                }
                                            }
                                            else
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

            private byte[]? _mimeTypeBytes;

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

            public IGremlinqClient Create(IGremlinQueryEnvironment environment)
            {
                var client = new ClientWebSocket();

                _webSocketOptionsConfiguration(client.Options);

                return new WebSocketGremlinqClient(this, client, environment);
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

            public IWebSocketGremlinqClientFactory ConfigureUri(Func<Uri, Uri> transformation) => new WebSocketGremlinqClientFactoryImpl<TBuffer>(transformation(_uri), _username, _password, _webSocketOptionsConfiguration, _bufferFactory);

            public IWebSocketGremlinqClientFactory ConfigureUsername(Func<string?, string?> transformation) => new WebSocketGremlinqClientFactoryImpl<TBuffer>(_uri, transformation(_username), _password, _webSocketOptionsConfiguration, _bufferFactory);

            public IWebSocketGremlinqClientFactory ConfigurePassword(Func<string?, string?> transformation) => new WebSocketGremlinqClientFactoryImpl<TBuffer>(_uri, _username, transformation(_password), _webSocketOptionsConfiguration, _bufferFactory);

            public IWebSocketGremlinqClientFactory WithMessageBufferFactory<TNewBuffer>(IMessageBufferFactory<TNewBuffer> factory) where TNewBuffer : IMemoryOwner<byte> => new WebSocketGremlinqClientFactoryImpl<TNewBuffer>(_uri, _username, _password, _webSocketOptionsConfiguration, factory);
        }

        public static readonly IWebSocketGremlinqClientFactory LocalHost = new WebSocketGremlinqClientFactoryImpl<GraphSon3MessageBuffer>(new Uri("ws://localhost:8182"), null, null, options => options.SetRequestHeader("User-Agent", UserAgent), MessageBufferFactory.GraphSon3);

        private static readonly string UserAgent = $"{typeof(IGremlinQueryBase).Assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product}/{typeof(WebSocketGremlinqClientFactory).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion} {Environment.OSVersion.VersionString};";
    }
}
