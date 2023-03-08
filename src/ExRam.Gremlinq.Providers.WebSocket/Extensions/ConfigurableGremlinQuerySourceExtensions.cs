using ExRam.Gremlinq.Core.Deserialization;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Providers.WebSocket;
using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core
{
    public static class ConfigurableGremlinQuerySourceExtensions
    {
        private sealed class WebSocketGremlinQueryExecutor : IGremlinQueryExecutor, IDisposable
        {
            private static readonly AsyncLocal<IGremlinQueryEnvironment?> _currentEnvironment = new();

            private readonly Dictionary<string, string> _aliasArgs;
            private readonly SmarterLazy<IGremlinClient> _lazyGremlinClient;

            public WebSocketGremlinQueryExecutor(
                GremlinServer gremlinServer,
                IGremlinClientFactory clientFactory,
                string alias = "g")
            {
                _aliasArgs = new Dictionary<string, string> { {"g", alias} };

                _lazyGremlinClient = new SmarterLazy<IGremlinClient>(
                    async logger =>
                    {
                        try
                        {
                            return await Task.Run(() => clientFactory.Create(
                                gremlinServer,
                                new JsonNetMessageSerializer(() => _currentEnvironment.Value ?? throw new InvalidOperationException()),
                                new ConnectionPoolSettings(),
                                static _ => { }));
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, $"Failure creating an instance of {nameof(IGremlinClient)}.");

                            throw;
                        }
                    });
            }

            public void Dispose()
            {
                _lazyGremlinClient.Dispose();
            }

            public IAsyncEnumerable<object> Execute(ISerializedGremlinQuery serializedQuery, IGremlinQueryEnvironment environment)
            {
                return AsyncEnumerable.Create(Core);

                async IAsyncEnumerator<object> Core(CancellationToken ct)
                {
                    var clientTask = _lazyGremlinClient.GetValue(environment.Logger);

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

                    var client = await clientTask;

                    Task<ResultSet<JToken>>? resultTask = null;

                    try
                    {
                        _currentEnvironment.Value = environment;

                        resultTask = client
                            .SubmitAsync<JToken>(requestMessage);
                    }
                    finally
                    {
                        _currentEnvironment.Value = null;
                    }

                    if (await resultTask.ConfigureAwait(false) is { } results)
                    {
                        foreach (var obj in results)
                        {
                            yield return obj;
                        }
                    }
                }
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
                    .ConfigureSerializer(ser => ser
                        .UseGraphSon3())
                    .ConfigureExecutor(executor => executor
                        .Log())
                    .ConfigureDeserializer(d => d
                        .AddNewtonsoftJson()));
        }
    }
}
