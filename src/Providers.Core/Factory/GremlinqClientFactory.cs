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

        public static IGremlinQueryExecutor ToExecutor(this IGremlinqClientFactory clientFactory) => new GremlinQueryExecutorImpl(clientFactory);

        public static IGremlinqClientFactory Log(this IGremlinqClientFactory clientFactory) => new ConfigureClientGremlinClientFactory(clientFactory, (client, environment) => client.Log(environment));
    }
}
