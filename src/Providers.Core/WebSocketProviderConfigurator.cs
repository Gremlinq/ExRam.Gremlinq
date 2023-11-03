using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;
using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Exceptions;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.Core
{
    public sealed class WebSocketProviderConfigurator : IWebSocketProviderConfigurator<WebSocketProviderConfigurator>
    {
        private sealed class WebSocketGremlinQueryExecutor : IGremlinQueryExecutor
        {
            private readonly IGremlinClientFactory _clientFactory;
            private readonly ConcurrentDictionary<IGremlinQueryEnvironment, IGremlinClient> _clients = new();

            public WebSocketGremlinQueryExecutor(IGremlinClientFactory clientFactory)
            {
                _clientFactory = clientFactory;
            }

            public IAsyncEnumerable<T> Execute<T>(GremlinQueryExecutionContext context)
            {
                return Core(this, context);

                async static IAsyncEnumerable<T> Core(WebSocketGremlinQueryExecutor @this, GremlinQueryExecutionContext context, [EnumeratorCancellation] CancellationToken ct = default)
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

                    ResultSet<object>? maybeResults;

                    try
                    {
                        maybeResults = await client
                            .SubmitAsync<object>(requestMessage, ct)
                            .ConfigureAwait(false);
                    }
                    catch (ConnectionClosedException ex)
                    {
                        throw new GremlinQueryExecutionException(context, ex);
                    }
                    catch (NoConnectionAvailableException ex)
                    {
                        throw new GremlinQueryExecutionException(context, ex);
                    }
                    catch (ResponseException ex)
                    {
                        throw new GremlinQueryExecutionException(context, ex);
                    }

                    if (maybeResults is { } results)
                    {
                        foreach (var obj in results)
                        {
                            yield return environment.Deserializer
                                .TransformTo<T>()
                                .From(obj, environment);
                        }
                    }
                }
            }
        }

        public static readonly WebSocketProviderConfigurator Default = new (GremlinqConfigurator.Identity, GremlinClientFactory.LocalHost);

        private readonly IGremlinClientFactory _clientFactory;
        private readonly GremlinqConfigurator _gremlinqConfigurator;

        private WebSocketProviderConfigurator(
            GremlinqConfigurator gremlinqConfigurator,
            IGremlinClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _gremlinqConfigurator = gremlinqConfigurator;
        }

        public WebSocketProviderConfigurator ConfigureClientFactory(Func<IGremlinClientFactory, IGremlinClientFactory> transformation) => new (
            _gremlinqConfigurator,
            transformation(_clientFactory));

        public WebSocketProviderConfigurator ConfigureQuerySource(Func<IGremlinQuerySource, IGremlinQuerySource> transformation) => new(
            _gremlinqConfigurator.ConfigureQuerySource(transformation),
            _clientFactory);

        public IGremlinQuerySource Transform(IGremlinQuerySource source) => _gremlinqConfigurator
            .Transform(source
                .ConfigureEnvironment(environment => environment
                    .UseExecutor(Build())));

        private IGremlinQueryExecutor Build() => new WebSocketGremlinQueryExecutor(_clientFactory.Log());
    }
}
