using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Net.WebSockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks.Sources;

using ExRam.Gremlinq.Core;

using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Exceptions;
using Gremlin.Net.Driver.Messages;

using static Gremlin.Net.Driver.Messages.ResponseStatusCode;

namespace ExRam.Gremlinq.Providers.Core
{
    public static class WebSocketGremlinqClientFactory
    {
        private sealed class WebSocketGremlinqClientFactoryImpl<TBinaryMessage> : IWebSocketGremlinqClientFactory
            where TBinaryMessage : IMemoryOwner<byte>
        {
            private sealed class WebSocketGremlinqClient : IGremlinqClient
            {
                private interface IChannel : IDisposable
                {
                    void Signal(TBinaryMessage buffer, Guid requestId, ResponseStatus responseStatus);
                }

                private readonly struct ResponseAndQueueUnion<T>
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
                        queue = _queue;
                        semaphore = _semaphore;

                        return queue is not null && semaphore is not null;
                    }

                    public static ResponseAndQueueUnion<T> From(ResponseMessage<T> response) => new(response);

                    public static ResponseAndQueueUnion<T> CreateQueue() => new(new(0), new());
                }

                private sealed class Channel<T> : IChannel, IAsyncEnumerable<ResponseMessage<T>>, IValueTaskSource<ResponseAndQueueUnion<T>?>
                {
                    private readonly WebSocketGremlinqClient _client;

                    private ValueTaskSourceCore<ResponseAndQueueUnion<T>?> _valueTaskSource;

                    public Channel(WebSocketGremlinqClient client)
                    {
                        _client = client;
                    }

                    public void Signal(TBinaryMessage buffer, Guid requestId, ResponseStatus responseStatus)
                    {
                        try
                        {
                            if (_client._environment.Deserializer.TryTransform(buffer, _client._environment, out ResponseMessagePayload<T> payload))
                            {
                                if (payload.Result is { } payloadResult)
                                    Signal(new ResponseMessage<T>(requestId, responseStatus, payloadResult));
                                else
                                    Dispose();
                            }
                            else
                                throw new InvalidOperationException($"Unable to convert byte array to a {nameof(ResponseMessage<T>)} for {typeof(T).FullName}.");
                        }
                        catch
                        {
                            using (this)
                            {
                                throw;
                            }
                        }
                    }

                    private void Signal(ResponseMessage<T> response)
                    {
                        while (true)
                        {
                            if (_valueTaskSource.GetStatus(0) > ValueTaskSourceStatus.Pending)
                            {
                                if (_valueTaskSource.GetResult(0) is { } union && union.TryGetQueue(out var semaphore, out var queue))
                                {
                                    queue.Enqueue(response);
                                    semaphore.Release();
                                }

                                return;
                            }

                            if (response.Status.Code is not PartialContent and not Authenticate)
                            {
                                if (_valueTaskSource.TrySetResult(ResponseAndQueueUnion<T>.From(response)))
                                    return;
                            }
                            else
                                _valueTaskSource.TrySetResult(ResponseAndQueueUnion<T>.CreateQueue());
                        }
                    }

                    public async IAsyncEnumerator<ResponseMessage<T>> GetAsyncEnumerator(CancellationToken ct = default)
                    {
                        await using (ct.Register(Dispose))
                        {
                            if (await new ValueTask<ResponseAndQueueUnion<T>?>(this, 0) is { } union)
                            {
                                if (union.TryGetResponse(out var response))
                                    yield return response;
                                else if (union.TryGetQueue(out var semaphore, out var queue))
                                {
                                    while (true)
                                    {
                                        await semaphore.WaitAsync(ct);

                                        if (queue.TryDequeue(out var queuedResponse))
                                        {
                                            if (queuedResponse.Status.Code is Authenticate)
                                            {
                                                try
                                                {
                                                    await _client.SendCore(_client._factory._authMessageFactory((IReadOnlyDictionary<string, object>)queuedResponse.Status.Attributes ?? ImmutableDictionary<string, object>.Empty), ct);
                                                }
                                                catch
                                                {
                                                    using (this)
                                                    {
                                                        throw;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                yield return queuedResponse;

                                                if (queuedResponse.Status.Code != PartialContent)
                                                    break;
                                            }
                                        }
                                    }
                                }
                                else
                                    throw new NotSupportedException();
                            }
                            else
                                throw new ObjectDisposedException(nameof(Channel<T>));
                        }
                    }

                    public void Dispose()
                    {
                        while (true)
                        {
                            if (_valueTaskSource.GetStatus(0) is var status and > ValueTaskSourceStatus.Pending)
                            {
                                if (status == ValueTaskSourceStatus.Succeeded && _valueTaskSource.GetResult(0) is { } union && union.TryGetQueue(out var semaphore, out _))
                                    semaphore.Dispose();

                                return;
                            }

                            if (_valueTaskSource.TrySetResult(null))
                                return;
                        }
                    }

                    ResponseAndQueueUnion<T>? IValueTaskSource<ResponseAndQueueUnion<T>?>.GetResult(short token) => _valueTaskSource.GetResult(token);

                    ValueTaskSourceStatus IValueTaskSource<ResponseAndQueueUnion<T>?>.GetStatus(short token) => _valueTaskSource.GetStatus(token);

                    void IValueTaskSource<ResponseAndQueueUnion<T>?>.OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags) => _valueTaskSource.OnCompleted(continuation, state, token, flags);
                }

                private record struct ResponseMessagePayload<T>(ResponseResult<T>? Result);

                private record struct ResponseMessageEnvelope(Guid? RequestId, ResponseStatus? Status);

                private readonly ClientWebSocket _client;
                private readonly SemaphoreSlim _sendLock = new(1);
                private readonly CancellationTokenSource _cts = new();
                private readonly IGremlinQueryEnvironment _environment;
                private readonly TaskCompletionSource<Task?> _loopTcs = new();
                private readonly ConcurrentDictionary<Guid, IChannel> _channels = new();
                private readonly WebSocketGremlinqClientFactoryImpl<TBinaryMessage> _factory;

                public WebSocketGremlinqClient(WebSocketGremlinqClientFactoryImpl<TBinaryMessage> factory, ClientWebSocket client, IGremlinQueryEnvironment environment)
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
                        if (@this._client.CloseStatus is not null)
                            throw new ObjectDisposedException(nameof(WebSocketGremlinqClient));

                        using (var channel = new Channel<T>(@this))
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
                                                try
                                                {
                                                    if (!await e.MoveNextAsync())
                                                        break;
                                                }
                                                catch (ObjectDisposedException ex)
                                                {
                                                    throw new OperationCanceledException(null, ex);
                                                }
                                            }
                                            catch
                                            {
                                                @this.Dispose();

                                                if (await @this._loopTcs.Task is { } task)
                                                    await task;

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
                            using (_cts)
                            {
                                _cts.Cancel();
                                _loopTcs.TrySetResult(null);
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
                                await _client.ConnectAsync(_factory._uri, ct);

                                _loopTcs.SetResult(Loop(_cts.Token));
                            }

                            if (_environment.Serializer.TryTransform(requestMessage, _environment, out TBinaryMessage? buffer))
                            {
                                using (buffer)
                                {
                                    await _client.SendAsync(buffer.Memory, WebSocketMessageType.Binary, true, ct);
                                }
                            }
                            else
                                throw new InvalidOperationException();
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
                            IMemoryOwner<byte>? bytes;

                            try
                            {
                                bytes = await _client.ReceiveAsync(ct);
                            }
                            catch (OperationCanceledException)
                            {
                                return;
                            }
                            catch (InvalidOperationException)
                            {
                                return;
                            }
                            catch (WebSocketException)
                            {
                                return;
                            }

                            if (_environment.Deserializer.TryTransform(bytes, _environment, out TBinaryMessage? binaryMessage))
                            {
                                using (binaryMessage)
                                {
                                    if (_environment.Deserializer.TryTransform(binaryMessage, _environment, out ResponseMessageEnvelope responseMessageEnvelope))
                                    {
                                        if (responseMessageEnvelope is { Status: { Code: var statusCode, Message: var message } responseStatus, RequestId: { } requestId })
                                        {
                                            if (_channels.TryGetValue(requestId, out var otherChannel))
                                                otherChannel.Signal(binaryMessage, requestId, responseStatus);
                                            else if (statusCode >= Unauthorized)
                                                throw new ResponseException(statusCode, ImmutableDictionary<string, object>.Empty, $"The server returned a response indicating failure, but the response could not be mapped to a request: {message}");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            public static readonly IWebSocketGremlinqClientFactory LocalHost = new WebSocketGremlinqClientFactoryImpl<TBinaryMessage>(
                new Uri("ws://localhost:8182"),
                () =>
                {
                    var client = new ClientWebSocket();
                    client.Options.SetRequestHeader("User-Agent", UserAgent);

                    return client;
                },
                _ => throw new NotSupportedException("Authentication credentials were requested from the server but were not configured."),
                (client, _) => client);

            private readonly Uri _uri;
            private readonly Func<ClientWebSocket> _clientWebSocketFactory;
            private readonly Func<IReadOnlyDictionary<string, object>, RequestMessage> _authMessageFactory;
            private readonly Func<IGremlinqClient, IGremlinQueryEnvironment, IGremlinqClient> _clientTransformation;

            private WebSocketGremlinqClientFactoryImpl(Uri uri, Func<ClientWebSocket> clientWebSocketFactory, Func<IReadOnlyDictionary<string, object>, RequestMessage> authMessageFactory, Func<IGremlinqClient, IGremlinQueryEnvironment, IGremlinqClient> clientTransformation)
            {
                if (uri.Scheme is not "ws" and not "wss")
                    throw new ArgumentException($"Expected {nameof(uri)}.{nameof(Uri.Scheme)} to be either \"ws\" or \"wss\".", nameof(uri));

                _uri = uri.EnsurePath();
                _authMessageFactory = authMessageFactory;
                _clientTransformation = clientTransformation;
                _clientWebSocketFactory = clientWebSocketFactory;
            }

            public IGremlinqClient Create(IGremlinQueryEnvironment environment) => _clientTransformation(new WebSocketGremlinqClient(this, _clientWebSocketFactory(), environment), environment);

            public IWebSocketGremlinqClientFactory ConfigureClientWebSocketFactory(Func<Func<ClientWebSocket>, Func<ClientWebSocket>> transformation) => new WebSocketGremlinqClientFactoryImpl<TBinaryMessage>(_uri, transformation(_clientWebSocketFactory), _authMessageFactory, _clientTransformation);

            public IWebSocketGremlinqClientFactory ConfigureUri(Func<Uri, Uri> transformation) => new WebSocketGremlinqClientFactoryImpl<TBinaryMessage>(transformation(_uri), _clientWebSocketFactory, _authMessageFactory, _clientTransformation);

            public IWebSocketGremlinqClientFactory WithBinaryMessage<TNewBuffer>() where TNewBuffer : IMemoryOwner<byte> => new WebSocketGremlinqClientFactoryImpl<TNewBuffer>(_uri, _clientWebSocketFactory, _authMessageFactory, _clientTransformation);

            public IWebSocketGremlinqClientFactory ConfigureAuthentication(Func<Func<IReadOnlyDictionary<string, object>, RequestMessage>, Func<IReadOnlyDictionary<string, object>, RequestMessage>> transformation) => new WebSocketGremlinqClientFactoryImpl<TBinaryMessage>(_uri, _clientWebSocketFactory, transformation(_authMessageFactory), _clientTransformation);

            public IWebSocketGremlinqClientFactory ConfigureClient(Func<IGremlinqClient, IGremlinQueryEnvironment, IGremlinqClient> clientTransformation) => new WebSocketGremlinqClientFactoryImpl<TBinaryMessage>(_uri, _clientWebSocketFactory, _authMessageFactory, (client, env) => clientTransformation(_clientTransformation(client, env), env));
        }

        public static readonly IWebSocketGremlinqClientFactory LocalHost = WebSocketGremlinqClientFactoryImpl<GraphSon3BinaryMessage>.LocalHost;

        public static IWebSocketGremlinqClientFactory WithPlainCredentials(this IWebSocketGremlinqClientFactory factory, string username, string password) => factory
            .ConfigureAuthentication(_ => _ => RequestMessage
                .Build(Tokens.OpsAuthentication)
                .Processor(Tokens.ProcessorTraversal)
                .AddArgument(Tokens.ArgsSasl, Convert.ToBase64String(Encoding.UTF8.GetBytes($"\0{username}\0{password}")))
                .Create());

        private static readonly string UserAgent = $"{typeof(IGremlinQueryBase).Assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product}/{typeof(WebSocketGremlinqClientFactory).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion} {Environment.OSVersion.VersionString};";
    }
}
