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
    public static class WebSocketGremlinqClientFactory
    {
        private sealed class WebSocketGremlinqClientFactoryImpl<TBinaryMessage> : IWebSocketGremlinqClientFactory
            where TBinaryMessage : IMemoryOwner<byte>
        {
            private readonly record struct ResponseTuple(TBinaryMessage BinaryMessage, Guid RequestId, ResponseStatus ResponseStatus);

            private readonly struct ResponseAndQueueUnion
            {
                private readonly ResponseTuple? _response;
                private readonly SemaphoreSlim? _semaphore;
                private readonly ConcurrentQueue<ResponseTuple>? _queue;

                private ResponseAndQueueUnion(SemaphoreSlim semaphore, ConcurrentQueue<ResponseTuple> queue)
                {
                    _queue = queue;
                    _semaphore = semaphore;
                }

                private ResponseAndQueueUnion(ResponseTuple response)
                {
                    _response = response;
                }

                public bool TryGetResponse([NotNullWhen(true)] out ResponseTuple response)
                {
                    if (_response is { } actualResponse)
                    {
                        response = actualResponse;
                        return true;
                    }

                    response = default;
                    return false;
                }

                public bool TryGetQueue([NotNullWhen(true)] out SemaphoreSlim? semaphore, [NotNullWhen(true)] out ConcurrentQueue<ResponseTuple>? queue)
                {
                    queue = _queue;
                    semaphore = _semaphore;

                    return queue is not null && semaphore is not null;
                }

                public static ResponseAndQueueUnion From(ResponseTuple response) => new(response);

                public static ResponseAndQueueUnion CreateQueue() => new(new(0), new());
            }

            private sealed class WebSocketGremlinqClient : DisposableBase, IGremlinqClient
            {
                private sealed class Channel : IDisposable, IAsyncEnumerable<ResponseTuple>, IValueTaskSource<ResponseAndQueueUnion?>
                {
                    private readonly WebSocketGremlinqClient _client;

                    private ValueTaskSourceCore<ResponseAndQueueUnion?> _valueTaskSource;

                    public Channel(WebSocketGremlinqClient client)
                    {
                        _client = client;
                    }

                    public void Signal(TBinaryMessage buffer, Guid requestId, ResponseStatus responseStatus) => Signal(new ResponseTuple(buffer, requestId, responseStatus));

                    public void SignalCompletion() => Signal(null);

                    public async IAsyncEnumerator<ResponseTuple> GetAsyncEnumerator(CancellationToken ct = default)
                    {
                        var ctRegistration = ct
                            .Register(static @this => ((Channel)@this!).SignalCompletion(), this);

                        try
                        {
                            if (await new ValueTask<ResponseAndQueueUnion?>(this, 0).ConfigureAwait(false) is { } union)
                            {
                                if (union.TryGetResponse(out var response))
                                    yield return response;
                                else if (union.TryGetQueue(out var semaphore, out var queue))
                                {
                                    while (true)
                                    {
                                        await semaphore
                                            .WaitAsync(ct)
                                            .ConfigureAwait(false);

                                        if (queue.TryDequeue(out var queuedResponse))
                                        {
                                            if (queuedResponse.ResponseStatus.Code is Authenticate)
                                            {
                                                await _client
                                                    .SendCore(_client._factory._authMessageFactory((IReadOnlyDictionary<string, object>)queuedResponse.ResponseStatus.Attributes ?? ImmutableDictionary<string, object>.Empty))
                                                    .ConfigureAwait(false);
                                            }
                                            else
                                            {
                                                yield return queuedResponse;

                                                if (queuedResponse.ResponseStatus.Code != PartialContent)
                                                    break;
                                            }
                                        }
                                        else
                                            yield break;
                                    }
                                }
                                else
                                    throw new NotSupportedException();
                            }
                            else
                                throw new ObjectDisposedException(nameof(Channel));
                        }
                        finally
                        {
                            await ctRegistration
                                .DisposeAsync()
                                .ConfigureAwait(false);
                        }
                    }

                    public void Dispose()
                    {
                        _valueTaskSource
                            .TrySetResult(null);

                        if (_valueTaskSource.GetResult(0) is { } union && union.TryGetQueue(out var semaphore, out var _))
                            semaphore.Dispose();
                    }

                    private void Signal(ResponseTuple? maybeResponseTuple)
                    {
                        while (true)
                        {
                            if (_valueTaskSource.GetStatus(0) > ValueTaskSourceStatus.Pending)
                            {
                                if (_valueTaskSource.GetResult(0) is { } union && union.TryGetQueue(out var semaphore, out var queue))
                                {
                                    if (maybeResponseTuple is { } response)
                                        queue.Enqueue(response);

                                    semaphore
                                        .Release();
                                }

                                return;
                            }
                            else
                            {
                                if (maybeResponseTuple is { } response)
                                {
                                    if (response.ResponseStatus.Code is not PartialContent and not Authenticate)
                                    {
                                        if (_valueTaskSource.TrySetResult(ResponseAndQueueUnion.From(response)))
                                            return;
                                    }
                                    else
                                        _valueTaskSource.TrySetResult(ResponseAndQueueUnion.CreateQueue());
                                }
                                else if (_valueTaskSource.TrySetResult(null))
                                    return;
                            }
                        }
                    }

                    ResponseAndQueueUnion? IValueTaskSource<ResponseAndQueueUnion?>.GetResult(short token) => _valueTaskSource.GetResult(token);

                    ValueTaskSourceStatus IValueTaskSource<ResponseAndQueueUnion?>.GetStatus(short token) => _valueTaskSource.GetStatus(token);

                    void IValueTaskSource<ResponseAndQueueUnion?>.OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags) => _valueTaskSource.OnCompleted(continuation, state, token, flags);
                }

                private record struct ResponseMessagePayload<T>(ResponseResult<T>? Result);

                private record struct ResponseMessageEnvelope(Guid? RequestId, ResponseStatus? Status);

                private readonly ClientWebSocket _client;
                private readonly SemaphoreSlim _sendLock = new(1);
                private readonly CancellationTokenSource _cts = new();
                private readonly IGremlinQueryEnvironment _environment;
                private readonly TaskCompletionSource<Task?> _loopTcs = new();
                private readonly ConcurrentDictionary<Guid, Channel> _channels = new();
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

                        using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, @this._cts.Token))
                        {
                            using (var channel = new Channel(@this))
                            {
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
                                await using (var e = channel.WithCancellation(linkedCts.Token).ConfigureAwait(false).GetAsyncEnumerator())
#pragma warning restore CA2007
                                {
                                    try
                                    {
                                        @this._channels
                                            .TryAdd(message.RequestId, channel);

                                        await @this
                                            .SendCore(message)
                                            .ConfigureAwait(false);

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

                                                if (await @this._loopTcs.Task.ConfigureAwait(false) is { } task)
                                                    await task.ConfigureAwait(false);

                                                throw;
                                            }

                                            var (binaryMessage, requestId, responseStatus) = e.Current;

                                            using (binaryMessage)
                                            {
                                                // Since the below yield return is what effectively yields control back to user code,
                                                // the receive loop may stall if continations were executed synchronously up to here and
                                                // user code blocks. Although technically not Gremlinq's fault, we take this measure.
                                                await Task.Yield();

                                                if (@this._environment.Deserializer.TryTransform(binaryMessage, @this._environment, out ResponseMessagePayload<T> payload))
                                                {
                                                    if (payload.Result is { } payloadResult)
                                                        yield return new ResponseMessage<T>(requestId, responseStatus, payloadResult);
                                                    else
                                                        yield break;
                                                }
                                                else
                                                    throw new InvalidOperationException($"Unable to convert byte array to a {nameof(ResponseMessage<>)} for {typeof(T).FullName}.");
                                            }
                                        }
                                    }
                                    finally
                                    {
                                        @this._channels
                                            .TryRemove(message.RequestId, out _);

                                        channel
                                            .SignalCompletion();

                                        while (await e.MoveNextAsync())
                                        {
                                            e.Current.BinaryMessage
                                                .Dispose();
                                        }
                                    }
                                }
                            }
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
                                    if (ct.IsCancellationRequested)
                                        break;

                                    if (_environment.Deserializer.TryTransform(bytes, _environment, out TBinaryMessage? binaryMessage))
                                    {
                                        try
                                        {
                                            if (_environment.Deserializer.TryTransform(binaryMessage, _environment, out ResponseMessageEnvelope responseMessageEnvelope))
                                            {
                                                if (responseMessageEnvelope is { Status: { } responseStatus, RequestId: { } requestId })
                                                {
                                                    if (_channels.TryGetValue(requestId, out var otherChannel))
                                                    {
                                                        otherChannel
                                                            .Signal(binaryMessage, requestId, responseStatus);

                                                        continue;
                                                    }
                                                }
                                            }

                                            binaryMessage
                                                .Dispose();
                                        }
                                        catch
                                        {
                                            using (binaryMessage)
                                            {
                                                throw;
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
