using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;

using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Exceptions;
using Gremlin.Net.Driver.Messages;

using Microsoft.Extensions.Logging;

using static Gremlin.Net.Driver.Messages.ResponseStatusCode;

namespace ExRam.Gremlinq.Providers.Core
{
    /// <summary>
    /// Provides factory methods and extension methods for <see cref="IGremlinqClientFactory"/>.
    /// </summary>
    public static class GremlinqClientFactory
    {
        private sealed class PoolGremlinqClientFactory<TBaseFactory> : IPoolGremlinqClientFactory<TBaseFactory>, IAsyncDisposable
            where TBaseFactory : IGremlinqClientFactory
        {
            private sealed class PoolGremlinqClient : IGremlinqClient
            {
                private sealed class PoolSlotGremlinqClient : IGremlinqClient
                {
                    private IGremlinqClient? _currentClient;
                    private readonly PoolGremlinqClient _outerClient;

                    public PoolSlotGremlinqClient(PoolGremlinqClient outerClient)
                    {
                        _outerClient = outerClient;
                    }

                    public IAsyncEnumerable<ResponseMessage<T>> SubmitAsync<T>(RequestMessage message) => GetOrCreateConnection().SubmitAsync<T>(message);

                    public PoolConnectionGremlinqClient GetOrCreateConnection()
                    {
                        while (true)
                        {
                            if (Volatile.Read(ref _currentClient) is { } currentConnection)
                                return currentConnection as PoolConnectionGremlinqClient ?? throw new ObjectDisposedException(nameof(PoolGremlinqClient));

                            var newConnection = new PoolConnectionGremlinqClient(
                                this,
                                _outerClient._baseFactory
                                    .Create(_outerClient._environment)
                                    .Throttle(_outerClient._maxInProcessPerConnection));

                            if (Interlocked.CompareExchange(ref _currentClient, newConnection, null) != null)
                                newConnection.Dispose();
                        }
                    }

                    public bool Holds(PoolConnectionGremlinqClient connection) => Volatile.Read(ref _currentClient) == connection;

                    public void Drop(PoolConnectionGremlinqClient connection)
                    {
                        using (connection)
                        {
                            Interlocked.CompareExchange(ref _currentClient, null, connection);
                        }
                    }

                    public void Dispose() => Interlocked.Exchange(ref _currentClient, GremlinqClient.Disposed)?.Dispose();
                }

                private sealed class PoolConnectionGremlinqClient : IGremlinqClient
                {
                    private readonly IGremlinqClient _client;
                    private readonly PoolSlotGremlinqClient _slot;

                    public PoolConnectionGremlinqClient(PoolSlotGremlinqClient slot, IGremlinqClient client)
                    {
                        _slot = slot;
                        _client = client;
                    }

                    public IAsyncEnumerable<ResponseMessage<T>> SubmitAsync<T>(RequestMessage message)
                    {
                        return Core(this, message);

                        static async IAsyncEnumerable<ResponseMessage<T>> Core(PoolConnectionGremlinqClient @this, RequestMessage message, [EnumeratorCancellation] CancellationToken ct = default)
                        {
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
                            await using (var e = @this._client.SubmitAsync<T>(message).WithCancellation(ct).ConfigureAwait(false).GetAsyncEnumerator())
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
                                        @this._slot.Drop(@this);

                                        throw;
                                    }

                                    yield return e.Current;
                                }
                            }
                        }
                    }

                    public bool IsCurrent => _slot.Holds(this);

                    public void Dispose() => _client.Dispose();
                }

                private readonly PoolSlotGremlinqClient[] _slots;
                private readonly IGremlinqClient[] _retryingSlots;
                private readonly int _maxInProcessPerConnection;
                private readonly IGremlinqClientFactory _baseFactory;
                private readonly IGremlinQueryEnvironment _environment;
                private readonly ConcurrentDictionary<string, PoolConnectionGremlinqClient> _sessions = new();

                private int _maxRequestsInUse;
                private int _currentRequestsInUse;
                private int _currentSlotIndex = -1;

                public PoolGremlinqClient(IGremlinqClientFactory baseFactory, int poolSize, int maxInProcessPerConnection, IGremlinQueryEnvironment environment)
                {
                    _baseFactory = baseFactory;
                    _environment = environment;
                    _slots = new PoolSlotGremlinqClient[poolSize];
                    _retryingSlots = new IGremlinqClient[poolSize];
                    _maxInProcessPerConnection = maxInProcessPerConnection;

                    for (var i = 0; i < poolSize; i++)
                    {
                        _slots[i] = new PoolSlotGremlinqClient(this);
                        _retryingSlots[i] = _slots[i]
                            .Retry(static (retry, ex) => ex is ObjectDisposedException && retry == 0);
                    }
                }

                public IAsyncEnumerable<ResponseMessage<T>> SubmitAsync<T>(RequestMessage message)
                {
                    return Core(message, this);

                    static async IAsyncEnumerable<ResponseMessage<T>> Core(RequestMessage message, PoolGremlinqClient @this, [EnumeratorCancellation] CancellationToken ct = default)
                    {
                        var currentRequestsInUse = Interlocked.Increment(ref @this._currentRequestsInUse);

                        try
                        {
                            var responses = message.Processor == Tokens.ProcessorSession && message.Arguments.TryGetValue(Tokens.ArgsSession, out var maybeSession) && maybeSession?.ToString() is { } sessionId
                                ? @this.SubmitInSession<T>(sessionId, message, currentRequestsInUse)
                                : @this._retryingSlots[@this.GetSlotIndex(currentRequestsInUse)].SubmitAsync<T>(message);

                            await foreach (var item in responses.WithCancellation(ct).ConfigureAwait(false))
                            {
                                yield return item;
                            }
                        }
                        finally
                        {
                            Interlocked.Decrement(ref @this._currentRequestsInUse);
                        }
                    }
                }

                public void Dispose()
                {
                    for (var i = 0; i < _slots.Length; i++)
                    {
                        _slots[i].Dispose();
                    }

                    _sessions.Clear();
                }

                private IAsyncEnumerable<ResponseMessage<T>> SubmitInSession<T>(string sessionId, RequestMessage message, int currentRequestsInUse)
                {
                    if (message.Operation == Tokens.OpsClose)
                    {
                        return _sessions.TryRemove(sessionId, out var boundConnection)
                            ? SubmitInSession<T>(sessionId, message, boundConnection)
                            : _slots[GetSlotIndex(currentRequestsInUse)].SubmitAsync<T>(message);
                    }

                    var connection = _sessions.GetOrAdd(
                        sessionId,
                        static (_, state) => state.@this._slots[state.@this.GetSlotIndex(state.currentRequestsInUse)].GetOrCreateConnection(),
                        (@this: this, currentRequestsInUse));

                    return SubmitInSession<T>(sessionId, message, connection);
                }

                private static IAsyncEnumerable<ResponseMessage<T>> SubmitInSession<T>(string sessionId, RequestMessage message, PoolConnectionGremlinqClient connection) => connection.IsCurrent
                    ? connection.SubmitAsync<T>(message)
                    : throw new InvalidOperationException($"The connection of session {sessionId} was closed.");

                private int GetSlotIndex(int currentRequestsInUse)
                {
                    while (true)
                    {
                        var maxRequestsInUse = Volatile.Read(ref _maxRequestsInUse);
                        var newMaxRequestsInUse = Math.Min(currentRequestsInUse, _slots.Length);

                        if (currentRequestsInUse < 0 || currentRequestsInUse <= maxRequestsInUse || newMaxRequestsInUse == maxRequestsInUse || Interlocked.CompareExchange(ref _maxRequestsInUse, newMaxRequestsInUse, maxRequestsInUse) == maxRequestsInUse)
                            return Math.Abs(Interlocked.Increment(ref _currentSlotIndex) % newMaxRequestsInUse);
                    }
                }
            }

            private readonly int _poolSize;
            private readonly TBaseFactory _baseFactory;
            private readonly int _maxInProcessPerConnection;
            private readonly Func<IGremlinqClient, IGremlinQueryEnvironment, IGremlinqClient> _clientTransformation;

            public PoolGremlinqClientFactory(TBaseFactory baseFactory, int poolSize, int maxInProcessPerConnection, Func<IGremlinqClient, IGremlinQueryEnvironment, IGremlinqClient> clientTransformation)
            {
                _poolSize = poolSize;
                _baseFactory = baseFactory;
                _clientTransformation = clientTransformation;
                _maxInProcessPerConnection = maxInProcessPerConnection;
            }

            public IPoolGremlinqClientFactory<TNewBaseFactory> ConfigureBaseFactory<TNewBaseFactory>(Func<TBaseFactory, TNewBaseFactory> transformation) where TNewBaseFactory : IGremlinqClientFactory => new PoolGremlinqClientFactory<TNewBaseFactory>(transformation(_baseFactory), _poolSize, _maxInProcessPerConnection, _clientTransformation);

            public IPoolGremlinqClientFactory<TBaseFactory> WithMaxInProcessPerConnection(int maxInProcessPerConnection) => maxInProcessPerConnection is > 0 and <= 64
                ? new PoolGremlinqClientFactory<TBaseFactory>(_baseFactory, _poolSize, maxInProcessPerConnection, _clientTransformation)
                : throw new ArgumentOutOfRangeException(nameof(maxInProcessPerConnection));

            public IPoolGremlinqClientFactory<TBaseFactory> WithPoolSize(int poolSize) => poolSize is > 0 and <= 8
                ? new PoolGremlinqClientFactory<TBaseFactory>(_baseFactory, poolSize, _maxInProcessPerConnection, _clientTransformation)
                : throw new ArgumentOutOfRangeException(nameof(poolSize));

            public IGremlinqClient Create(IGremlinQueryEnvironment environment) => _clientTransformation(new PoolGremlinqClient(_baseFactory, _poolSize, _maxInProcessPerConnection, environment), environment);

            public IPoolGremlinqClientFactory<TBaseFactory> ConfigureClient(Func<IGremlinqClient, IGremlinQueryEnvironment, IGremlinqClient> clientTransformation) => new PoolGremlinqClientFactory<TBaseFactory>(_baseFactory, _poolSize, _maxInProcessPerConnection, (client, env) => clientTransformation(_clientTransformation(client, env), env));

            public ValueTask DisposeAsync() => _baseFactory is IAsyncDisposable asyncDisposable
                ? asyncDisposable.DisposeAsync()
                : ValueTask.CompletedTask;
        }

        private sealed class GremlinQueryExecutorImpl : IGremlinQueryExecutor, IAsyncDisposable
        {
            private static class MetaResponseExecution<T>
            {
                public static readonly Func<GremlinQueryExecutorImpl, GremlinQueryExecutionContext, IAsyncEnumerable<T>>? Delegate = typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(MetaResponse<>)
                    ? typeof(GremlinQueryExecutorImpl)
                        .GetMethod(nameof(ExecuteMetaResponse), BindingFlags.NonPublic | BindingFlags.Static)!
                        .MakeGenericMethod(typeof(T).GenericTypeArguments[0])
                        .CreateDelegate<Func<GremlinQueryExecutorImpl, GremlinQueryExecutionContext, IAsyncEnumerable<T>>>()
                    : null;
            }

            private readonly IGremlinqClientFactory _clientFactory;
            private readonly ConcurrentDictionary<IGremlinQueryEnvironment, IGremlinqClient> _clients = new();

            public GremlinQueryExecutorImpl(IGremlinqClientFactory clientFactory)
            {
                _clientFactory = clientFactory;
            }

            public ValueTask DisposeAsync() => _clientFactory is IAsyncDisposable asyncDisposable
                ? asyncDisposable.DisposeAsync()
                : ValueTask.CompletedTask;

            public IAsyncEnumerable<T> Execute<T>(GremlinQueryExecutionContext context)
            {
                if (MetaResponseExecution<T>.Delegate is { } execute)
                    return execute(this, context);

                return Execute<T, T>(
                    context,
                    static (context, _, response) => response switch
                    {
                        { Status: { Code: var code and not Success and not NoContent and not PartialContent and not Authenticate, Attributes: var attributes, Message: var message } } => throw new GremlinQueryExecutionException(context, new ResponseException(code, attributes, $"{code}: {message}")),
                        _ => response.Result.Data ?? []
                    });
            }

            private async IAsyncEnumerable<TTransformedResult> Execute<TResult, TTransformedResult>(GremlinQueryExecutionContext context, Func<GremlinQueryExecutionContext, RequestMessage, ResponseMessage<List<TResult>>, IEnumerable<TTransformedResult>> resultTransformation, [EnumeratorCancellation] CancellationToken ct = default)
            {
                var environment = context.Query
                    .AsAdmin()
                    .Environment;

                var client = _clients.GetOrAdd(
                    environment,
                    static (environment, executor) => executor._clientFactory.Create(environment),
                    this);

                var requestMessage = environment
                    .Serializer
                    .TransformTo<RequestMessage>()
                    .From(context.Query, environment)
                    .Rebuild()
                    .OverrideRequestId(context.ExecutionId)
                    .Create();

#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
                await using (var enumerator = client.SubmitAsync<List<TResult>>(requestMessage).GetAsyncEnumerator(ct))
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
                {
                    while (true)
                    {
                        IEnumerable<TTransformedResult> data;

                        try
                        {
                            try
                            {
                                if (!await enumerator.MoveNextAsync().ConfigureAwait(false))
                                    break;
                            }
                            catch (Exception ex) when (ex is not ArgumentException and not OperationCanceledException)
                            {
                                throw new GremlinQueryExecutionException(context, ex);
                            }

                            data = resultTransformation(context, requestMessage, enumerator.Current);
                        }
                        catch (Exception ex)
                        {
                            environment.Logger
                                .LogError(ex, "Execution of Gremlin query {executionId} failed.", context.ExecutionId);

                            throw;
                        }

                        foreach (var obj in data)
                        {
                            yield return obj;
                        }
                    }
                }
            }

            private static IAsyncEnumerable<MetaResponse<T>> ExecuteMetaResponse<T>(GremlinQueryExecutorImpl executor, GremlinQueryExecutionContext context) => executor.Execute<T, MetaResponse<T>>(context, static (_, request, response) => [new MetaResponse<T>(request.RequestId, response.Result.Data?.ToArray(), response.Status)]);
        }

        /// <summary>
        /// Configures created clients by applying a transformation that does not depend on the environment.
        /// </summary>
        /// <typeparam name="TClientFactory">The concrete client factory type.</typeparam>
        /// <param name="clientFactory">The client factory to configure.</param>
        /// <param name="clientTransformation">A function that transforms the created client.</param>
        public static TClientFactory ConfigureClient<TClientFactory>(this TClientFactory clientFactory, Func<IGremlinqClient, IGremlinqClient> clientTransformation)
            where TClientFactory : IGremlinqClientFactory<TClientFactory>
        {
            ArgumentNullException.ThrowIfNull(clientTransformation);

            return clientFactory.ConfigureClient((client, _) => clientTransformation(client));
        }

        /// <summary>
        /// Wraps the client factory in a connection pool with default pool size and concurrency settings.
        /// </summary>
        /// <typeparam name="TBaseFactory">The type of the underlying client factory.</typeparam>
        /// <param name="baseFactory">The client factory to pool.</param>
        public static IPoolGremlinqClientFactory<TBaseFactory> Pool<TBaseFactory>(this TBaseFactory baseFactory)
            where TBaseFactory : IGremlinqClientFactory => new PoolGremlinqClientFactory<TBaseFactory>(baseFactory, 8, 16, static (client, _) => client);

        /// <summary>
        /// Configures the client factory to log Gremlin queries using the environment's logger.
        /// </summary>
        /// <typeparam name="TClientFactory">The concrete client factory type.</typeparam>
        /// <param name="clientFactory">The client factory to configure.</param>
        public static TClientFactory Log<TClientFactory>(this TClientFactory clientFactory)
            where TClientFactory : IGremlinqClientFactory<TClientFactory> => clientFactory.ConfigureClient((client, environment) => client.Log(environment));

        /// <summary>
        /// Creates a <see cref="IGremlinQueryExecutor"/> from this client factory.
        /// </summary>
        /// <param name="clientFactory">The client factory to create an executor from.</param>
        public static IGremlinQueryExecutor ToExecutor(this IGremlinqClientFactory clientFactory)
        {
            ArgumentNullException.ThrowIfNull(clientFactory);

            return new GremlinQueryExecutorImpl(clientFactory);
        }
    }
}
