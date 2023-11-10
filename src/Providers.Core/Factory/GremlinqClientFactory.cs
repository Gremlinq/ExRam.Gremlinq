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
        private sealed class ConfigureClientGremlinClientFactory : IGremlinqClientFactory
        {
            private readonly IGremlinqClientFactory _baseFactory;
            private readonly Func<IGremlinqClient, IGremlinQueryEnvironment, IGremlinqClient> _clientTransformation;

            public ConfigureClientGremlinClientFactory(IGremlinqClientFactory baseFactory, Func<IGremlinqClient, IGremlinQueryEnvironment, IGremlinqClient> clientTransformation)
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
                private int _connectionIndex = 0;
                private readonly IGremlinqClient?[] _clients;
                private readonly IGremlinqClientFactory _baseFactory;
                private readonly IGremlinQueryEnvironment _environment;

                public PoolGremlinqClient(IGremlinqClientFactory baseFactory, int poolSize, IGremlinQueryEnvironment environment)
                {
                    _baseFactory = baseFactory;
                    _environment = environment;

                    _clients = new IGremlinqClient?[poolSize];
                }

                public IAsyncEnumerable<ResponseMessage<T>> SubmitAsync<T>(RequestMessage message)
                {
                    return Core(message, this);

                    static async IAsyncEnumerable<ResponseMessage<T>> Core(RequestMessage message, PoolGremlinqClient @this, [EnumeratorCancellation] CancellationToken ct = default)
                    {
                        var slot = Math.Abs((Interlocked.Increment(ref @this._connectionIndex) - 1) % @this._clients.Length);

                        while (true)
                        {
                            var maybeClient = @this._clients[slot];

                            if (maybeClient is { } client)
                            {
                                await using (var e = client.SubmitAsync<T>(message).WithCancellation(ct).GetAsyncEnumerator())
                                {
                                    while (true)
                                    {
                                        try
                                        {
                                            if (!await e.MoveNextAsync())
                                                break;
                                        }
                                        catch (Exception)
                                        {
                                            using (client)
                                            {
                                                Interlocked.CompareExchange(ref @this._clients[slot], null, client);
                                            }

                                            throw;
                                        }

                                        yield return e.Current;
                                    }
                                }

                                yield break;
                            }
                            else
                                Interlocked.CompareExchange(ref @this._clients[slot], @this._baseFactory.Create(@this._environment), null);
                        }
                    }
                }

                public void Dispose()
                {
                    foreach (var client in _clients)
                    {
                        client?.Dispose();
                    }
                }
            }

            private readonly int _poolSize;
            private readonly TBaseFactory _baseFactory;
            private readonly int _maxInProcessPerConnection;

            public PoolGremlinqClientFactory(TBaseFactory baseFactory) : this(baseFactory, 8, 16)
            {
            }

            public PoolGremlinqClientFactory(TBaseFactory baseFactory, int poolSize, int maxInProcessPerConnection)
            {
                _poolSize = poolSize;
                _baseFactory = baseFactory;
                _maxInProcessPerConnection = maxInProcessPerConnection;
            }

            public IPoolGremlinqClientFactory<TBaseFactory> ConfigureBaseFactory(Func<TBaseFactory, TBaseFactory> transformation) => new PoolGremlinqClientFactory<TBaseFactory>(transformation(_baseFactory));

            public IPoolGremlinqClientFactory<TBaseFactory> WithMaxInProcessPerConnection(int maxInProcessPerConnection) => new PoolGremlinqClientFactory<TBaseFactory>(_baseFactory, _poolSize, maxInProcessPerConnection);

            public IPoolGremlinqClientFactory<TBaseFactory> WithPoolSize(int poolSize) => new PoolGremlinqClientFactory<TBaseFactory>(_baseFactory, poolSize, _maxInProcessPerConnection);

            public IGremlinqClient Create(IGremlinQueryEnvironment environment) => new PoolGremlinqClient(_baseFactory, _poolSize, environment);
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

                async static IAsyncEnumerable<T> Core(GremlinQueryExecutorImpl @this, GremlinQueryExecutionContext context, [EnumeratorCancellation] CancellationToken ct = default)
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

                    await using (var e = client.SubmitAsync<List<T>>(requestMessage).GetAsyncEnumerator(ct))
                    {
                        while (true)
                        {
                            try
                            {
                                if (!await e.MoveNextAsync())
                                    break;
                            }
                            catch (ConnectionClosedException ex)
                            {
                                throw new GremlinQueryExecutionException(context, ex);
                            }
                            catch (NoConnectionAvailableException ex)
                            {
                                throw new GremlinQueryExecutionException(context, ex);
                            }

                            var response = e.Current;

                            if (response is { Status: { Code: { } code } status } && code is not Success and not NoContent and not PartialContent and not Authenticate)
                                throw new GremlinQueryExecutionException(context, new ResponseException(code, status.Attributes, $"{status.Code}: {status.Message}"));

                            if (response is { Result.Data: { } data })
                            {
                                foreach (var obj in data)
                                {
                                    yield return obj;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static IGremlinqClientFactory ConfigureClient(this IGremlinqClientFactory clientFactory, Func<IGremlinqClient, IGremlinqClient> clientTransformation) => new ConfigureClientGremlinClientFactory(clientFactory, (client, _) => clientTransformation(client));

        public static IPoolGremlinqClientFactory<TBaseFactory> Pool<TBaseFactory>(this TBaseFactory baseFactory)
            where TBaseFactory : IGremlinqClientFactory => new PoolGremlinqClientFactory<TBaseFactory>(baseFactory);

        public static IGremlinqClientFactory Log(this IGremlinqClientFactory clientFactory) => new ConfigureClientGremlinClientFactory(clientFactory, (client, environment) => client.Log(environment));

        public static IGremlinQueryExecutor ToExecutor(this IGremlinqClientFactory clientFactory) => new GremlinQueryExecutorImpl(clientFactory);
    }
}
