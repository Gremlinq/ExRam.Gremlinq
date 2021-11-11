using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core
{
    public static class ConfigurableGremlinQuerySourceExtensions
    {
        private sealed class WebSocketGremlinQueryExecutor : IGremlinQueryExecutor, IDisposable
        {
            private static readonly ConditionalWeakTable<IGremlinQueryEnvironment, Action<object, Guid>> Loggers = new();

            private readonly string _alias;
            private readonly Dictionary<string, string> _aliasArgs;
            private readonly SmarterLazy<IGremlinClient> _lazyGremlinClient;

            public WebSocketGremlinQueryExecutor(
                Func<CancellationToken, Task<IGremlinClient>> clientFactory,
                string alias = "g")
            {
                _alias = alias;
                _aliasArgs = new Dictionary<string, string> { {"g", _alias} };
                _lazyGremlinClient = new SmarterLazy<IGremlinClient>(
                    async logger =>
                    {
                        try
                        {
                            return await clientFactory(default);
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
                    var results = default(ResultSet<JToken>);
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

                    try
                    {
                        var task = client
                            .SubmitAsync<JToken>(requestMessage);

                        var logger = Loggers.GetValue(
                            environment,
                            environment => GetLoggingFunction(environment));

                        logger(serializedQuery, requestMessage.RequestId);

                        results = await task
                            .ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        environment.Logger.LogError($"Error executing Gremlin query with {nameof(RequestMessage.RequestId)} {requestMessage.RequestId}: {ex}");

                        throw;
                    }

                    if (results != null)
                    {
                        foreach (var obj in results)
                        {
                            yield return obj;
                        }
                    }
                }
            }

            private static Action<object, Guid> GetLoggingFunction(IGremlinQueryEnvironment environment)
            {
                var logLevel = environment.Options.GetValue(WebSocketGremlinqOptions.QueryLogLogLevel);
                var verbosity = environment.Options.GetValue(WebSocketGremlinqOptions.QueryLogVerbosity);
                var formatting = environment.Options.GetValue(WebSocketGremlinqOptions.QueryLogFormatting);
                var groovyFormatting = environment.Options.GetValue(WebSocketGremlinqOptions.QueryLogGroovyFormatting);

                return (serializedQuery, requestId) =>
                {
                    if (environment.Logger.IsEnabled(logLevel))
                    {
                        var gremlinQuery = serializedQuery switch
                        {
                            Bytecode bytecode => bytecode.ToGroovy(groovyFormatting),
                            GroovyGremlinQuery groovyGremlinQuery => groovyFormatting == GroovyFormatting.Inline
                                ? groovyGremlinQuery.Inline()
                                : groovyGremlinQuery,
                            _ => throw new ArgumentException($"Cannot handle serialized query of type {serializedQuery.GetType()}.")
                        };

                        environment.Logger.Log(
                            logLevel,
                            "Executing Gremlin query {0}.",
                            JsonConvert.SerializeObject(
                                new
                                {
                                    RequestId = requestId,
                                    gremlinQuery.Script,
                                    Bindings = (verbosity & QueryLogVerbosity.IncludeBindings) > QueryLogVerbosity.QueryOnly
                                        ? gremlinQuery.Bindings
                                        : null
                                },
                                formatting));
                    }
                };
            }
        }

        private sealed class WebSocketConfigurator : IWebSocketConfigurator
        {
            private readonly Uri? _uri;
            private readonly string _alias;
            private readonly IMessageSerializer _serializer;
            private readonly IGremlinClientFactory _clientFactory;
            private readonly (string username, string password)? _auth;
            private readonly Func<IGremlinClient, IGremlinClient> _clientTransformation;

            public WebSocketConfigurator(
                Uri? uri,
                IGremlinClientFactory clientFactory,
                IMessageSerializer serializer,
                (string username, string password)? auth,
                string alias,
                Func<IGremlinClient, IGremlinClient> clientTransformation)
            {
                _uri = uri;
                _auth = auth;
                _alias = alias;
                _serializer = serializer;
                _clientFactory = clientFactory;
                _clientTransformation = clientTransformation;
            }

            public IWebSocketConfigurator At(Uri uri) => new WebSocketConfigurator(uri, _clientFactory, _serializer, _auth, _alias, _clientTransformation);

            public IWebSocketConfigurator ConfigureGremlinClient(Func<IGremlinClient, IGremlinClient> transformation) => new WebSocketConfigurator(_uri, _clientFactory, _serializer, _auth, _alias, _ => transformation(_clientTransformation(_)));

            public IWebSocketConfigurator ConfigureGremlinClientFactory(Func<IGremlinClientFactory, IGremlinClientFactory> transformation) => new WebSocketConfigurator(_uri, transformation(_clientFactory), _serializer, _auth, _alias, _clientTransformation);

            public IWebSocketConfigurator ConfigureMessageSerializer(Func<IMessageSerializer, IMessageSerializer> transformation) => new WebSocketConfigurator(_uri, _clientFactory, transformation(_serializer), _auth, _alias, _clientTransformation);

            public IWebSocketConfigurator AuthenticateBy(string username, string password) => new WebSocketConfigurator(_uri, _clientFactory, _serializer, (username, password), _alias, _clientTransformation);

            public IWebSocketConfigurator SetAlias(string alias) => new WebSocketConfigurator(_uri, _clientFactory, _serializer, _auth, alias, _clientTransformation);

            public IGremlinQuerySource Transform(IGremlinQuerySource source)
            {
                return source
                    .ConfigureEnvironment(environment => environment
                        .UseExecutor(Build()));
            }

            private IGremlinQueryExecutor Build()
            {
                if (_uri == null)
                    throw new InvalidOperationException($"No valid Gremlin endpoint found. Configure {nameof(GremlinQuerySource.g)} with {nameof(UseWebSocket)} and use {nameof(At)} on the configurator to set a valid Gremlin endpoint.");

                if (!"ws".Equals(_uri.Scheme, StringComparison.OrdinalIgnoreCase) && !"wss".Equals(_uri.Scheme, StringComparison.OrdinalIgnoreCase))
                    throw new ArgumentException("Expected the Uri-Scheme to be either \"ws\" or \"wss\".");

                return new WebSocketGremlinQueryExecutor(
                    async ct => _clientTransformation(await Task.Run(
                        () => _clientFactory.Create(
                            new GremlinServer(
                                (_uri.Host + _uri.AbsolutePath).TrimEnd('/'),
                                _uri.Port,
                                "wss".Equals(_uri.Scheme, StringComparison.OrdinalIgnoreCase),
                                _auth?.username,
                                _auth?.password),
                            _serializer,
                            null,
                            null),
                        ct)),
                    _alias);
            }
        }

        public static IGremlinQuerySource UseWebSocket(
            this IConfigurableGremlinQuerySource source,
            Func<IWebSocketConfigurator, IGremlinQuerySourceTransformation> configuratorTransformation)
        {
            var configurator = new WebSocketConfigurator(
                default,
                GremlinClientFactory.Default,
                JsonNetMessageSerializer.GraphSON3,
                null,
                "g",
                _ => _);

            return configuratorTransformation(configurator)
                .Transform(source.ConfigureEnvironment(_ => _))
                .ConfigureEnvironment(environment => environment
                    .ConfigureDeserializer(d => d
                        .ConfigureFragmentDeserializer(f => f
                            .AddNewtonsoftJson())));
        }
    }
}
