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

            public IAsyncEnumerable<T> Execute<T>(IGremlinQueryBase query)
            {
                return AsyncEnumerable.Create(Core);

                async IAsyncEnumerator<T> Core(CancellationToken ct)
                {
                    var environment = query
                        .AsAdmin()
                        .Environment;

                    var client = _clients
                        .GetOrAdd(
                            environment,
                            static (environment, @this) => @this._clientFactory.Create(
                                environment,
                                @this._gremlinServer,
                                new DefaultMessageSerializer(environment),
                                new ConnectionPoolSettings(),
                                static _ => { }),
                            this);

                    var requestMessage = environment
                        .Serializer
                        .TransformTo<RequestMessage>()
                        .From(query, environment);

                    var maybeResults = await client
                        .SubmitAsync<object>(requestMessage, ct)
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
