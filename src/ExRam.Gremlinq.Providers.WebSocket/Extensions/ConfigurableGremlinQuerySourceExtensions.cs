using System.Collections.Concurrent;
using ExRam.Gremlinq.Core.Deserialization;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Providers.WebSocket;
using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;
using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core
{
    public static class ConfigurableGremlinQuerySourceExtensions
    {
        private sealed class WebSocketGremlinQueryExecutor : IGremlinQueryExecutor, IDisposable
        {
            private readonly GremlinServer _gremlinServer;
            private readonly IGremlinClientFactory _clientFactory;
            private readonly Dictionary<string, string> _aliasArgs;
            private readonly ConcurrentDictionary<IGremlinQueryEnvironment, IGremlinClient> _clients = new();

            public WebSocketGremlinQueryExecutor(
                GremlinServer gremlinServer,
                IGremlinClientFactory clientFactory,
                string alias = "g") 
            {
                _gremlinServer = gremlinServer;
                _clientFactory = clientFactory;
                _aliasArgs = new Dictionary<string, string> { {"g", alias} };
            }

            public IAsyncEnumerable<object> Execute(ISerializedGremlinQuery serializedQuery, IGremlinQueryEnvironment environment)
            {
                return AsyncEnumerable.Create(Core);

                async IAsyncEnumerator<object> Core(CancellationToken ct)
                {
                    var client = _clients
                        .GetOrAdd(
                            environment,
                            static (environment, @this) => @this._clientFactory.Create(
                                @this._gremlinServer,
                                new DefaultMessageSerializer(environment),
                                new ConnectionPoolSettings(),
                                static _ => { }),
                            this);

                    if (!Guid.TryParse(serializedQuery.Id, out var requestId))
                    {
                        requestId = Guid.NewGuid();
                        environment.Logger.LogInformation($"Mapping query id {serializedQuery.Id} to request id {requestId}.");
                    }

                    var requestMessage = serializedQuery switch
                    {
                        GroovyGremlinQuery groovyScript => RequestMessage
                            .Build(Tokens.OpsEval)
                            .AddArgument(Tokens.ArgsGremlin, groovyScript.Script)
                            .AddArgument(Tokens.ArgsAliases, _aliasArgs)
                            .AddArgument(Tokens.ArgsBindings, groovyScript.Bindings)
                            .OverrideRequestId(requestId)
                            .Create(),
                        BytecodeGremlinQuery bytecodeQuery => RequestMessage
                            .Build(Tokens.OpsBytecode)
                            .Processor(Tokens.ProcessorTraversal)
                            .AddArgument(Tokens.ArgsGremlin, bytecodeQuery.Bytecode)
                            .AddArgument(Tokens.ArgsAliases, _aliasArgs)
                            .OverrideRequestId(requestId)
                            .Create(),
                        _ => throw new ArgumentException($"Cannot handle serialized query of type {serializedQuery.GetType()}.")
                    };

                    var maybeResults = await client
                        .SubmitAsync<object>(requestMessage)
                        .ConfigureAwait(false);

                    if (maybeResults is { } results)
                    {
                        foreach (var obj in results)
                        {
                            yield return obj;
                        }
                    }
                }
            }

            public void Dispose()
            {
                //TODO: Dispose clients ?
            }
        }

        private sealed class WebSocketConfigurator : IWebSocketConfigurator
        {
            private readonly string _alias;
            private readonly GremlinServer _gremlinServer;
            private readonly IGremlinClientFactory _clientFactory;

            public WebSocketConfigurator(
                GremlinServer gremlinServer,
                IGremlinClientFactory clientFactory,
                string alias)
            {
                _alias = alias;
                _gremlinServer = gremlinServer;
                _clientFactory = clientFactory;
            }

            public IWebSocketConfigurator ConfigureServer(Func<GremlinServer, GremlinServer> transformation) => new WebSocketConfigurator(transformation(_gremlinServer), _clientFactory, _alias);

            public IWebSocketConfigurator ConfigureClientFactory(Func<IGremlinClientFactory, IGremlinClientFactory> transformation) => new WebSocketConfigurator(_gremlinServer, transformation(_clientFactory), _alias);

            public IWebSocketConfigurator ConfigureAlias(Func<string, string> transformation) => new WebSocketConfigurator(_gremlinServer, _clientFactory, transformation(_alias));

            public IGremlinQuerySource Transform(IGremlinQuerySource source) => source
                .ConfigureEnvironment(environment => environment
                    .UseExecutor(Build()));

            private IGremlinQueryExecutor Build() => (!"ws".Equals(_gremlinServer.Uri.Scheme, StringComparison.OrdinalIgnoreCase) && !"wss".Equals(_gremlinServer.Uri.Scheme, StringComparison.OrdinalIgnoreCase))
                ? throw new ArgumentException("Expected the Uri-Scheme to be either \"ws\" or \"wss\".")
                : new WebSocketGremlinQueryExecutor(
                    _gremlinServer,
                    _clientFactory,
                    _alias);
        }

        public static IGremlinQuerySource UseWebSocket(
            this IConfigurableGremlinQuerySource source,
            Func<IWebSocketConfigurator, IGremlinQuerySourceTransformation> configuratorTransformation)
        {
            var configurator = new WebSocketConfigurator(
                new GremlinServer(),
                GremlinClientFactory.Default,
                "g");

            return configuratorTransformation(configurator)
                .Transform(source
                    .ConfigureEnvironment(_ => _))
                .ConfigureEnvironment(environment => environment
                    .UseGraphSon3()
                    .ConfigureExecutor(executor => executor
                        .Log()));
        }
    }
}
