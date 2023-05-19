using System.Collections.Concurrent;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Transformation;
using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.Core
{
    public sealed class WebSocketProviderConfigurator : IWebSocketProviderConfigurator<WebSocketProviderConfigurator>
    {
        private sealed class WebSocketGremlinQueryExecutor : IGremlinQueryExecutor
        {
            private sealed class ExecutionEnumerable<T> : IAsyncEnumerable<T>
            {
                private readonly GremlinQueryExecutionContext _context;
                private readonly WebSocketGremlinQueryExecutor _executor;

                public ExecutionEnumerable(WebSocketGremlinQueryExecutor executor, GremlinQueryExecutionContext context)
                {
                    _context = context;
                    _executor = executor;
                }

                public async IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
                {
                    var environment = _context.Query
                        .AsAdmin()
                        .Environment;

                    var client = _executor._clients
                        .GetOrAdd(
                            environment,
                            static (environment, executor) => executor._clientFactory.Create(
                                environment,
                                executor._gremlinServer,
                                new DefaultMessageSerializer(environment),
                                new ConnectionPoolSettings(),
                                static _ => { }),
                            _executor);

                    var requestMessageBuilder = environment
                        .Serializer
                        .TransformTo<RequestMessage.Builder>()
                        .From(_context.Query, environment);

                    var requestMessage = requestMessageBuilder
                        .OverrideRequestId(_context.ExecutionId)
                        .Create();

                    var maybeResults = await client
                        .SubmitAsync<object>(requestMessage, cancellationToken)
                        .ConfigureAwait(false);

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

            private readonly GremlinServer _gremlinServer;
            private readonly IGremlinClientFactory _clientFactory;
            private readonly ConcurrentDictionary<IGremlinQueryEnvironment, IGremlinClient> _clients = new();

            public WebSocketGremlinQueryExecutor(
                GremlinServer gremlinServer,
                IGremlinClientFactory clientFactory)
            {
                _gremlinServer = gremlinServer;
                _clientFactory = clientFactory;
            }

            public IAsyncEnumerable<T> Execute<T>(GremlinQueryExecutionContext context) => new ExecutionEnumerable<T>(this, context);
        }

        private readonly GremlinServer _gremlinServer;
        private readonly IGremlinClientFactory _clientFactory;
        private readonly GremlinqConfigurator _gremlinqConfigurator;

        public WebSocketProviderConfigurator() : this(
            new GremlinqConfigurator(),
            new GremlinServer(),
            GremlinClientFactory.Default)
        {
        }

        public WebSocketProviderConfigurator(
            GremlinqConfigurator gremlinqConfigurator,
            GremlinServer gremlinServer,
            IGremlinClientFactory clientFactory)
        {
            _gremlinServer = gremlinServer;
            _clientFactory = clientFactory;
            _gremlinqConfigurator = gremlinqConfigurator;
        }

        public WebSocketProviderConfigurator ConfigureServer(Func<GremlinServer, GremlinServer> transformation) => new (
            _gremlinqConfigurator,
            transformation(_gremlinServer),
            _clientFactory);

        public WebSocketProviderConfigurator ConfigureClientFactory(Func<IGremlinClientFactory, IGremlinClientFactory> transformation) => new (
            _gremlinqConfigurator,
            _gremlinServer,
            transformation(_clientFactory));

        public WebSocketProviderConfigurator ConfigureQuerySource(Func<IGremlinQuerySource, IGremlinQuerySource> transformation) => new(
            _gremlinqConfigurator.ConfigureQuerySource(transformation),
            _gremlinServer,
            _clientFactory);

        public IGremlinQuerySource Transform(IGremlinQuerySource source) => _gremlinqConfigurator
            .Transform(source
                .ConfigureEnvironment(environment => environment
                    .UseExecutor(Build())));

        private IGremlinQueryExecutor Build() => !"ws".Equals(_gremlinServer.Uri.Scheme, StringComparison.OrdinalIgnoreCase) && !"wss".Equals(_gremlinServer.Uri.Scheme, StringComparison.OrdinalIgnoreCase)
            ? throw new ArgumentException("Expected the Uri-Scheme to be either \"ws\" or \"wss\".")
            : new WebSocketGremlinQueryExecutor(
                _gremlinServer,
                _clientFactory.Log());
    }
}
