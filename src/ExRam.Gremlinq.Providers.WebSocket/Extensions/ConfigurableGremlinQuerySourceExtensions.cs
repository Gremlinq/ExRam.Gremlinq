using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core.Deserialization;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Providers.WebSocket;
using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;
using Gremlin.Net.Process.Traversal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core
{
    public static class ConfigurableGremlinQuerySourceExtensions
    {
        private sealed class WebSocketGremlinQueryExecutor : IGremlinQueryExecutor, IDisposable
        {
            private readonly string _alias;
            private readonly Dictionary<string, string> _aliasArgs;
            private readonly SmarterLazy<IGremlinClient> _lazyGremlinClient;

            public WebSocketGremlinQueryExecutor(
                GremlinServer gremlinServer,
                IGremlinClientFactory clientFactory,
                string alias = "g")
            {
                _alias = alias;
                _aliasArgs = new Dictionary<string, string> { {"g", _alias} };

                _lazyGremlinClient = new SmarterLazy<IGremlinClient>(
                    async logger =>
                    {
                        try
                        {
                            return await Task.Run(() => clientFactory.Create(
                                gremlinServer,
                                JsonNetMessageSerializer.GraphSON3,
                                null,
                                null,
                                null));
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, $"Failure creating a {nameof(GremlinClient)} instance.");

                            throw;
                        }
                    });
            }

            public void Dispose()
            {
                _lazyGremlinClient.Dispose();
            }

            public IAsyncEnumerable<object> Execute(object serializedQuery, IGremlinQueryEnvironment environment)
            {
                return AsyncEnumerable.Create(Core);

                async IAsyncEnumerator<object> Core(CancellationToken ct)
                {
                    var maybeResults = default(ResultSet<JToken>?);
                    var client = await _lazyGremlinClient.GetValue(environment.Logger);

                    var requestMessage = serializedQuery switch
                    {
                        GroovyGremlinQuery groovyScript => RequestMessage
                            .Build(Tokens.OpsEval)
                            .AddArgument(Tokens.ArgsGremlin, $"{_alias}.{groovyScript.Script}")
                            .AddArgument(Tokens.ArgsBindings, groovyScript.Bindings)
                            .Create(),
                        Bytecode bytecode => RequestMessage
                            .Build(Tokens.OpsBytecode)
                            .Processor(Tokens.ProcessorTraversal)
                            .AddArgument(Tokens.ArgsGremlin, bytecode)
                            .AddArgument(Tokens.ArgsAliases, _aliasArgs)
                            .Create(),
                        _ => throw new ArgumentException($"Cannot handle serialized query of type {serializedQuery.GetType()}.")
                    };

                    maybeResults = await client
                        .SubmitAsync<JToken>(requestMessage)
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

            public IGremlinQuerySource Transform(IGremlinQuerySource source)
            {
                return source
                    .ConfigureEnvironment(environment => environment
                        .UseExecutor(Build()));
            }

            private IGremlinQueryExecutor Build()
            {
                if (!"ws".Equals(_gremlinServer.Uri.Scheme, StringComparison.OrdinalIgnoreCase) && !"wss".Equals(_gremlinServer.Uri.Scheme, StringComparison.OrdinalIgnoreCase))
                    throw new ArgumentException("Expected the Uri-Scheme to be either \"ws\" or \"wss\".");

                return new WebSocketGremlinQueryExecutor(
                    _gremlinServer,
                    _clientFactory,
                    _alias);
            }
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
                    .ConfigureExecutor(executor => executor
                        .Log())
                    .ConfigureDeserializer(d => d
                        .ConfigureFragmentDeserializer(f => f
                            .AddNewtonsoftJson())));
        }
    }
}
