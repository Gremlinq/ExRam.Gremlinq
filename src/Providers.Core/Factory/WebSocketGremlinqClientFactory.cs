using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Net.WebSockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks.Sources;

using CommunityToolkit.HighPerformance.Buffers;

using ExRam.Gremlinq.Core;

using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;

using static Gremlin.Net.Driver.Messages.ResponseStatusCode;

namespace ExRam.Gremlinq.Providers.Core
{
    /// <summary>
    /// Provides factory methods and extension methods for <see cref="IWebSocketGremlinqClientFactory"/>.
    /// </summary>
    public static class WebSocketGremlinqClientFactory
    {
        private sealed class WebSocketGremlinqClientFactoryImpl<TBinaryMessage> : IWebSocketGremlinqClientFactory
            where TBinaryMessage : IMemoryOwner<byte>
        {
            private sealed class WebSocketGremlinqClient : DisposableBase, IGremlinqClient
            {
                private interface IChannel
                {
                    void Signal(TBinaryMessage buffer, Guid requestId, ResponseStatus responseStatus);
                }

                private readonly struct ResponseOrException<T>
                {
                    private readonly object? _value;

                    private ResponseOrException(Exception? exception) : this()
                    {
                        _value = exception;
                    }

                    private ResponseOrException(ResponseMessage<T> response) : this()
                    {
                        _value = response;
                    }

                    public Exception? Exception => _value as Exception;

                    public ResponseMessage<T> Response => (_value as ResponseMessage<T>) ?? throw (Exception)_value!;

                    public static ResponseOrException<T> From(Exception ex) => new(ex);

                    public static ResponseOrException<T> From(ResponseMessage<T> response) => new(response);
                }

                private readonly struct SingleOrQueue<T>
                {
                    private readonly SemaphoreSlim? _semaphore;
                    private readonly ResponseOrException<T>? _response;
                    private readonly ConcurrentQueue<ResponseOrException<T>>? _queue;

                    private SingleOrQueue(SemaphoreSlim semaphore, ConcurrentQueue<ResponseOrException<T>> queue)
                    {
                        _queue = queue;
                        _semaphore = semaphore;
                    }

                    private SingleOrQueue(ResponseOrException<T> response)
                    {
                        _response = response;
                    }

                    public bool TryGetResponse([NotNullWhen(true)] out ResponseOrException<T> response)
                    {
                        if (_response is { } availableResponse)
                        {
                            response = availableResponse;
                            return true;
                        }

                        response = default;
                        return false;
                    }

                    public bool TryGetQueue([NotNullWhen(true)] out SemaphoreSlim? semaphore, [NotNullWhen(true)] out ConcurrentQueue<ResponseOrException<T>>? queue)
                    {
                        queue = _queue;
                        semaphore = _semaphore;

                        return queue is not null && semaphore is not null;
                    }

                    public static SingleOrQueue<T> From(ResponseOrException<T> response) => new(response);

                    public static SingleOrQueue<T> CreateQueue() => new(new(0), new());
                }

                private sealed class Channel<T> : IChannel, IAsyncEnumerable<ResponseMessage<T>>, IValueTaskSource<SingleOrQueue<T>>
                {
                    private readonly WebSocketGremlinqClient _client;

                    private ValueTaskSourceCore<SingleOrQueue<T>> _valueTaskSource;

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
                                    Signal(ResponseOrException<T>.From(new ResponseMessage<T>(requestId, responseStatus, payloadResult)));
                                else
                                    Signal(ResponseOrException<T>.From(new ObjectDisposedException(nameof(Channel<>))));
                            }
                            else
                                throw new InvalidOperationException($"Unable to convert byte array to a {nameof(ResponseMessage<>)} for {typeof(T).FullName}.");
                        }
                        catch (Exception ex)
                        {
                            Signal(ResponseOrException<T>.From(ex));
                        }
                    }

                    public async IAsyncEnumerator<ResponseMessage<T>> GetAsyncEnumerator(CancellationToken ct = default)
                    {
                        var clientCtRegistration = _client._cts.Token
                            .UnsafeRegister(static @this => ((Channel<T>)@this!).Signal(ResponseOrException<T>.From(new ObjectDisposedException(nameof(Channel<>)))), this);

                        try
                        {
                            var ctRegistration = ct
                                .UnsafeRegister(static (@this, ct) => ((Channel<T>)@this!).Signal(ResponseOrException<T>.From(new OperationCanceledException(ct))), this);

                            try
                            {
                                var union = await new ValueTask<SingleOrQueue<T>>(this, 0).ConfigureAwait(false);

                                if (union.TryGetResponse(out var responseUnion))
                                    yield return responseUnion.Response;
                                else if (union.TryGetQueue(out var semaphore, out var queue))
                                {
                                    using (semaphore)
                                    {
                                        while (true)
                                        {
                                            await semaphore
                                                .WaitAsync(ct)
                                                .ConfigureAwait(false);

                                            if (queue.TryDequeue(out var queuedResponseUnion))
                                            {
                                                var queuedResponse = queuedResponseUnion.Response;

                                                if (queuedResponse.Status.Code is Authenticate)
                                                {
                                                    await _client
                                                        .SendCore(_client._factory._authMessageFactory((IReadOnlyDictionary<string, object>)queuedResponse.Status.Attributes ?? ImmutableDictionary<string, object>.Empty))
                                                        .ConfigureAwait(false);
                                                }
                                                else
                                                {
                                                    yield return queuedResponse;

                                                    if (queuedResponse.Status.Code != PartialContent)
                                                        break;
                                                }
                                            }
                                            else
                                                yield break;
                                        }
                                    }
                                }
                                else
                                    throw new NotSupportedException();
                            }
                            finally
                            {
                                await ctRegistration
                                    .DisposeAsync()
                                    .ConfigureAwait(false);
                            }
                        }
                        finally
                        {
                            await clientCtRegistration
                                .DisposeAsync()
                                .ConfigureAwait(false);
                        }
                    }

                    private void Signal(ResponseOrException<T> responseUnion)
                    {
                        while (true)
                        {
                            if (_valueTaskSource.GetStatus(0) > ValueTaskSourceStatus.Pending)
                            {
                                if (_valueTaskSource.GetResult(0) is { } union && union.TryGetQueue(out var semaphore, out var queue))
                                {
                                    queue.Enqueue(responseUnion);
                                    semaphore.Release();
                                }

                                return;
                            }
                            else
                            {
                                if (responseUnion.Exception is not null || responseUnion.Response.Status.Code is not PartialContent and not Authenticate)
                                {
                                    if (_valueTaskSource.TrySetResult(SingleOrQueue<T>.From(responseUnion)))
                                        return;
                                }
                                else
                                    _valueTaskSource.TrySetResult(SingleOrQueue<T>.CreateQueue());
                            }
                        }
                    }

                    SingleOrQueue<T> IValueTaskSource<SingleOrQueue<T>>.GetResult(short token) => _valueTaskSource.GetResult(token);

                    ValueTaskSourceStatus IValueTaskSource<SingleOrQueue<T>>.GetStatus(short token) => _valueTaskSource.GetStatus(token);

                    void IValueTaskSource<SingleOrQueue<T>>.OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags)
                    {
                        ArgumentNullException.ThrowIfNull(continuation);

                        _valueTaskSource.OnCompleted(continuation, state, token, flags);
                    }
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

                        var channel = new Channel<T>(@this);

                        @this._channels
                            .TryAdd(message.RequestId, channel);

                        try
                        {
                            await @this
                                .SendCore(message)
                                .ConfigureAwait(false);

#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
                            await using (var e = channel.WithCancellation(ct).ConfigureAwait(false).GetAsyncEnumerator())
#pragma warning restore CA2007
                            {
                                while (true)
                                {
                                    try
                                    {
                                        if (!await e.MoveNextAsync())
                                            break;
                                    }
                                    catch (Exception ex) when (ex is not OperationCanceledException)
                                    {
                                        @this.Dispose();

                                        if (await @this._loopTcs.Task.ConfigureAwait(false) is { } task)
                                            await task.ConfigureAwait(false);

                                        throw;
                                    }

                                    // Since the below yield return is what effectively yields control back to user code,
                                    // the receive loop may stall if user code blocks. Although technically not Gremlinq's
                                    // fault, we take this measure.
                                    await Task.Yield();

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

                protected override void DisposeImpl()
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

                private async Task SendCore(RequestMessage requestMessage)
                {
                    await _sendLock
                        .WaitAsync(_cts.Token)
                        .ConfigureAwait(false);

                    try
                    {
                        try
                        {
                            if (_client.State == WebSocketState.None)
                            {
                                await _client
                                    .ConnectAsync(_factory._uri, _cts.Token)
                                    .ConfigureAwait(false);

                                _loopTcs.SetResult(Loop());
                            }

                            if (_environment.Serializer.TryTransform(requestMessage, _environment, out TBinaryMessage? buffer))
                            {
                                using (buffer)
                                {
                                    await _client
                                        .SendAsync(buffer.Memory, WebSocketMessageType.Binary, true, _cts.Token)
                                        .ConfigureAwait(false);
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

                private async Task Loop()
                {
                    var ct = _cts.Token;

                    using (this)
                    {
                        Task<MemoryOwner<byte>>? maybeReceiveTask = null;

                        while (true)
                        {
                            MemoryOwner<byte>? maybeBytes = null;

                            try
                            {
                                if (maybeReceiveTask is { } receiveTask)
                                {
                                    maybeBytes = await receiveTask
                                        .ConfigureAwait(false);
                                }
                                
                                if (!ct.IsCancellationRequested)
                                    maybeReceiveTask = _client.ReceiveAsync(ct);
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

                            try
                            {
                                if (maybeBytes is { } bytes)
                                {
                                    using (bytes)
                                    {
                                        if (ct.IsCancellationRequested)
                                            break;

                                        if (_environment.Deserializer.TryTransform(bytes, _environment, out TBinaryMessage? binaryMessage))
                                        {
                                            using (binaryMessage)
                                            {
                                                if (_environment.Deserializer.TryTransform(binaryMessage, _environment, out ResponseMessageEnvelope responseMessageEnvelope))
                                                {
                                                    if (responseMessageEnvelope is { Status: { } responseStatus, RequestId: { } requestId })
                                                    {
                                                        if (_channels.TryGetValue(requestId, out var otherChannel))
                                                            otherChannel.Signal(binaryMessage, requestId, responseStatus);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                if (maybeReceiveTask is { } receiveTask)
                                    (await receiveTask.ConfigureAwait(false)).Dispose();

                                throw;
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

        /// <summary>
        /// A default <see cref="IWebSocketGremlinqClientFactory"/> configured to connect to <c>ws://localhost:8182</c> using GraphSON v3.
        /// </summary>
        public static readonly IWebSocketGremlinqClientFactory LocalHost = WebSocketGremlinqClientFactoryImpl<GraphSon3BinaryMessage>.LocalHost;

        /// <summary>
        /// Configures the WebSocket client factory to use plain text (SASL) authentication.
        /// </summary>
        /// <param name="factory">The client factory to configure.</param>
        /// <param name="username">The authentication username.</param>
        /// <param name="password">The authentication password.</param>
        public static IWebSocketGremlinqClientFactory WithPlainCredentials(this IWebSocketGremlinqClientFactory factory, string username, string password)
        {
            ArgumentNullException.ThrowIfNull(factory);
            ArgumentNullException.ThrowIfNull(username);
            ArgumentNullException.ThrowIfNull(password);

            return factory
                .ConfigureAuthentication(_ => _ => RequestMessage
                    .Build(Tokens.OpsAuthentication)
                    .Processor(Tokens.ProcessorTraversal)
                    .AddArgument(Tokens.ArgsSasl, Convert.ToBase64String(Encoding.UTF8.GetBytes($"\0{username}\0{password}")))
                    .Create());
        }

        private static readonly string UserAgent = $"{typeof(IGremlinQueryBase).Assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product}/{typeof(WebSocketGremlinqClientFactory).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion} {Environment.OSVersion.VersionString};";
    }
}
