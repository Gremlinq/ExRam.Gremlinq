using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;

using Gremlin.Net.Driver.Exceptions;
using Gremlin.Net.Driver.Messages;

using static Gremlin.Net.Driver.Messages.ResponseStatusCode;

namespace ExRam.Gremlinq.Providers.Core
{
    public static class GremlinqClientFactory
    {
        private sealed class ConfigureClientGremlinqClientFactory : IGremlinqClientFactory
        {
            private readonly IGremlinqClientFactory _baseFactory;
            private readonly Func<IGremlinqClient, IGremlinQueryEnvironment, IGremlinqClient> _clientTransformation;

            public ConfigureClientGremlinqClientFactory(IGremlinqClientFactory baseFactory, Func<IGremlinqClient, IGremlinQueryEnvironment, IGremlinqClient> clientTransformation)
            {
                _baseFactory = baseFactory;
                _clientTransformation = clientTransformation;
            }

            public IGremlinqClient Create(IGremlinQueryEnvironment environment) => _clientTransformation(_baseFactory.Create(environment), environment);
        }

        private sealed class PoolGremlinqClientFactory<TBaseFactory> : IPoolGremlinqClientFactory<TBaseFactory>
            where TBaseFactory : IGremlinqClientFactory
        {
            private sealed class PoolGremlinqClient : IGremlinqClient
            {
                private readonly IGremlinqClient?[] _slots;
                private readonly int _maxInProcessPerConnection;
                private readonly IGremlinqClientFactory _baseFactory;
                private readonly IGremlinQueryEnvironment _environment;

                private int _currentRequestsInUse;
                private int _maxRequestsInUse = 0;
                private int _currentSlotIndex = -1;

                public PoolGremlinqClient(IGremlinqClientFactory baseFactory, int poolSize, int maxInProcessPerConnection, IGremlinQueryEnvironment environment)
                {
                    _baseFactory = baseFactory;
                    _environment = environment;
                    _slots = new IGremlinqClient?[poolSize];
                    _maxInProcessPerConnection = maxInProcessPerConnection;
                }

                public IAsyncEnumerable<ResponseMessage<T>> SubmitAsync<T>(RequestMessage message)
                {
                    return Core(message, this);

                    static async IAsyncEnumerable<ResponseMessage<T>> Core(RequestMessage message, PoolGremlinqClient @this, [EnumeratorCancellation] CancellationToken ct = default)
                    {
                        var currentRequestsInUse = Interlocked.Increment(ref @this._currentRequestsInUse);

                        try
                        {
                            while (true)
                            {
                                var maxRequestsInUse = Volatile.Read(ref @this._maxRequestsInUse);
                                var newMaxRequestsInUse = Math.Min(currentRequestsInUse, @this._slots.Length);

                                if (currentRequestsInUse < 0 || currentRequestsInUse <= maxRequestsInUse || newMaxRequestsInUse == maxRequestsInUse || Interlocked.CompareExchange(ref @this._maxRequestsInUse, newMaxRequestsInUse, maxRequestsInUse) == maxRequestsInUse)
                                {
                                    var slotIndex = Math.Abs(Interlocked.Increment(ref @this._currentSlotIndex) % newMaxRequestsInUse);

                                    while (true)
                                    {
                                        if (Volatile.Read(ref @this._slots[slotIndex]) is { } client)
                                        {
                                            if (client == GremlinqClient.Disposed)
                                                throw new ObjectDisposedException(nameof(PoolGremlinqClient));

                                            await using (var e = client.SubmitAsync<T>(message).WithCancellation(ct).GetAsyncEnumerator())
                                            {
                                                while (true)
                                                {
                                                    try
                                                    {
                                                        if (!await e.MoveNextAsync())
                                                            break;
                                                    }
                                                    catch
                                                    {
                                                        using (client)
                                                        {
                                                            Interlocked.CompareExchange(ref @this._slots[slotIndex], null, client);
                                                        }

                                                        throw;
                                                    }

                                                    yield return e.Current;
                                                }
                                            }

                                            break;
                                        }
                                        else
                                        {
                                            var newClient = @this._baseFactory
                                                .Create(@this._environment)
                                                .Throttle(@this._maxInProcessPerConnection);

                                            if (Interlocked.CompareExchange(ref @this._slots[slotIndex], newClient, null) != null)
                                                newClient.Dispose();
                                        }
                                    }

                                    break;
                                }
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
                        Interlocked.Exchange(ref _slots[i], GremlinqClient.Disposed)?.Dispose();
                    }
                }
            }

            private readonly int _poolSize;
            private readonly TBaseFactory _baseFactory;
            private readonly int _maxInProcessPerConnection;

            public PoolGremlinqClientFactory(TBaseFactory baseFactory) : this(baseFactory, 8, 16)
            {
            }

            private PoolGremlinqClientFactory(TBaseFactory baseFactory, int poolSize, int maxInProcessPerConnection)
            {
                _poolSize = poolSize;
                _baseFactory = baseFactory;
                _maxInProcessPerConnection = maxInProcessPerConnection;
            }

            public IPoolGremlinqClientFactory<TBaseFactory> ConfigureBaseFactory(Func<TBaseFactory, TBaseFactory> transformation) => new PoolGremlinqClientFactory<TBaseFactory>(transformation(_baseFactory));

            public IPoolGremlinqClientFactory<TBaseFactory> WithMaxInProcessPerConnection(int maxInProcessPerConnection) => maxInProcessPerConnection is > 0 and <= 64
                ? new PoolGremlinqClientFactory<TBaseFactory>(_baseFactory, _poolSize, maxInProcessPerConnection)
                : throw new ArgumentOutOfRangeException(nameof(maxInProcessPerConnection));

            public IPoolGremlinqClientFactory<TBaseFactory> WithPoolSize(int poolSize) => poolSize is > 0 and <= 8
                ? new PoolGremlinqClientFactory<TBaseFactory>(_baseFactory, poolSize, _maxInProcessPerConnection)
                : throw new ArgumentOutOfRangeException(nameof(poolSize));

            public IGremlinqClient Create(IGremlinQueryEnvironment environment) => new PoolGremlinqClient(_baseFactory, _poolSize, _maxInProcessPerConnection, environment);
        }

        private sealed class GremlinQueryExecutorImpl : IGremlinQueryExecutor
        {
            private readonly IGremlinqClientFactory _clientFactory;
            private readonly ConcurrentDictionary<IGremlinQueryEnvironment, IGremlinqClient> _clients = new();

            public GremlinQueryExecutorImpl(IGremlinqClientFactory clientFactory)
            {
                _clientFactory = clientFactory;
            }

            public IAsyncEnumerable<T> Execute<T>(GremlinQueryExecutionContext context)
            {
                return Core(this, context);

                static async IAsyncEnumerable<T> Core(GremlinQueryExecutorImpl @this, GremlinQueryExecutionContext context, [EnumeratorCancellation] CancellationToken ct = default)
                {
                    var environment = context.Query
                        .AsAdmin()
                        .Environment;

                    var client = @this._clients.GetOrAdd(
                        environment,
                        static (environment, executor) => executor._clientFactory.Create(environment),
                        @this);

                    var requestMessage = environment
                        .Serializer
                        .TransformTo<RequestMessage>()
                        .From(context.Query, environment)
                        .Rebuild()
                        .OverrideRequestId(context.ExecutionId)
                        .Create();

                    await foreach (var response in client.SubmitAsync<List<T>>(requestMessage).Catch(ex => ex is not ArgumentException ? new GremlinQueryExecutionException(context, ex) : ex).WithCancellation(ct))
                    {
                        switch (response)
                        {
                            case { Status: { Code: var code and not Success and not NoContent and not PartialContent and not Authenticate } status }:
                                throw new GremlinQueryExecutionException(context, new ResponseException(code, status.Attributes, $"{status.Code}: {status.Message}"));
                            case { Result.Data: { } data }:
                            {
                                foreach (var obj in data)
                                {
                                    yield return obj;
                                }

                                break;
                            }
                        }
                    }
                }
            }
        }

        public static IGremlinqClientFactory ConfigureClient(this IGremlinqClientFactory clientFactory, Func<IGremlinqClient, IGremlinqClient> clientTransformation) => new ConfigureClientGremlinqClientFactory(clientFactory, (client, _) => clientTransformation(client));

        public static IPoolGremlinqClientFactory<TBaseFactory> Pool<TBaseFactory>(this TBaseFactory baseFactory)
            where TBaseFactory : IGremlinqClientFactory => new PoolGremlinqClientFactory<TBaseFactory>(baseFactory);

        public static IGremlinqClientFactory Log(this IGremlinqClientFactory clientFactory) => new ConfigureClientGremlinqClientFactory(clientFactory, (client, environment) => client.Log(environment));

        public static IGremlinQueryExecutor ToExecutor(this IGremlinqClientFactory clientFactory) => new GremlinQueryExecutorImpl(clientFactory);
    }
}
