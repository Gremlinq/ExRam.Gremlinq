﻿using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;

using Gremlin.Net.Driver.Exceptions;
using Gremlin.Net.Driver.Messages;

using Microsoft.Extensions.Logging;

using static Gremlin.Net.Driver.Messages.ResponseStatusCode;

namespace ExRam.Gremlinq.Providers.Core
{
    public static class GremlinqClientFactory
    {
        private sealed class PoolGremlinqClientFactory<TBaseFactory> : IPoolGremlinqClientFactory<TBaseFactory>
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
                    
                    public IAsyncEnumerable<ResponseMessage<T>> SubmitAsync<T>(RequestMessage message)
                    {
                        return Core(this, message);

                        static async IAsyncEnumerable<ResponseMessage<T>> Core(PoolSlotGremlinqClient @this, RequestMessage message, [EnumeratorCancellation] CancellationToken ct = default)
                        {
                            while (true)
                            {
                                if (Volatile.Read(ref @this._currentClient) is { } client)
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
                                                    Interlocked.CompareExchange(ref @this._currentClient, null, client);
                                                }

                                                throw;
                                            }

                                            yield return e.Current;
                                        }

                                        break;
                                    }
                                }
                                else
                                {
                                    var newClient = @this._outerClient._baseFactory
                                        .Create(@this._outerClient._environment)
                                        .Throttle(@this._outerClient._maxInProcessPerConnection);

                                    if (Interlocked.CompareExchange(ref @this._currentClient, newClient, null) != null)
                                        newClient.Dispose();
                                }
                            }
                        }
                    }

                    public void Dispose()
                    {
                        Interlocked.Exchange(ref _currentClient, GremlinqClient.Disposed)?.Dispose();
                    }
                }

                private readonly IGremlinqClient[] _slots;
                private readonly int _maxInProcessPerConnection;
                private readonly IGremlinqClientFactory _baseFactory;
                private readonly IGremlinQueryEnvironment _environment;

                private int _maxRequestsInUse;
                private int _currentRequestsInUse;
                private int _currentSlotIndex = -1;

                public PoolGremlinqClient(IGremlinqClientFactory baseFactory, int poolSize, int maxInProcessPerConnection, IGremlinQueryEnvironment environment)
                {
                    _baseFactory = baseFactory;
                    _environment = environment;
                    _slots = new IGremlinqClient[poolSize];
                    _maxInProcessPerConnection = maxInProcessPerConnection;

                    for (var i = 0; i < poolSize; i++)
                    {
                        _slots[i] = new PoolSlotGremlinqClient(this)
                            .Retry((retry, ex) => ex is ObjectDisposedException && retry == 0);
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
                            while (true)
                            {
                                var maxRequestsInUse = Volatile.Read(ref @this._maxRequestsInUse);
                                var newMaxRequestsInUse = Math.Min(currentRequestsInUse, @this._slots.Length);

                                if (currentRequestsInUse < 0 || currentRequestsInUse <= maxRequestsInUse || newMaxRequestsInUse == maxRequestsInUse || Interlocked.CompareExchange(ref @this._maxRequestsInUse, newMaxRequestsInUse, maxRequestsInUse) == maxRequestsInUse)
                                {
                                    var slotIndex = Math.Abs(Interlocked.Increment(ref @this._currentSlotIndex) % newMaxRequestsInUse);
                                    var client = @this._slots[slotIndex];

                                    await foreach (var item in client.SubmitAsync<T>(message).WithCancellation(ct))
                                    {
                                        yield return item;
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
                        _slots[i].Dispose();
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

                    var enumerable = client
                        .SubmitAsync<List<T>>(requestMessage)
                        .Catch(
                            static (ex, context) => ex is not ArgumentException ? new GremlinQueryExecutionException(context, ex) : ex,
                            context);

                    await foreach (var response in enumerable.WithCancellation(ct))
                    {
                        switch (response)
                        {
                            case { Status: { Code: var code and not Success and not NoContent and not PartialContent and not Authenticate } status }:
                            {
                                throw new GremlinQueryExecutionException(context, new ResponseException(code, status.Attributes, $"{status.Code}: {status.Message}"));
                            }
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

        public static TClientFactory ConfigureClient<TClientFactory>(this TClientFactory clientFactory, Func<IGremlinqClient, IGremlinqClient> clientTransformation)
            where TClientFactory : IGremlinqClientFactory<TClientFactory> => clientFactory.ConfigureClient((client, _) => clientTransformation(client));

        public static IPoolGremlinqClientFactory<TBaseFactory> Pool<TBaseFactory>(this TBaseFactory baseFactory)
            where TBaseFactory : IGremlinqClientFactory => new PoolGremlinqClientFactory<TBaseFactory>(baseFactory, 8, 16, static (client, _) => client);

        public static TClientFactory Log<TClientFactory>(this TClientFactory clientFactory)
            where TClientFactory : IGremlinqClientFactory<TClientFactory> => clientFactory.ConfigureClient((client, environment) => client.Log(environment));

        public static IGremlinQueryExecutor ToExecutor(this IGremlinqClientFactory clientFactory) => new GremlinQueryExecutorImpl(clientFactory);
    }
}
