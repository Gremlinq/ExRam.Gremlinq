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
        private sealed class LoggingGremlinQueryExecutor : IGremlinQueryExecutor
        {
            private static readonly ConditionalWeakTable<IGremlinQueryEnvironment, Action<object, Guid>> Loggers = new();

            private readonly IGremlinQueryExecutor _executor;

            public LoggingGremlinQueryExecutor(IGremlinQueryExecutor executor)
            {
                _executor = executor;
            }

            public IAsyncEnumerable<object> Execute(object serializedQuery, IGremlinQueryEnvironment environment)
            {
                return AsyncEnumerable.Create(Core);

                async IAsyncEnumerator<object> Core(CancellationToken ct)
                {
                    var requestId = Guid.NewGuid();

                    await using (var enumerator = _executor.Execute(serializedQuery, environment).GetAsyncEnumerator(ct))
                    {
                        try
                        {
                            var moveNext = enumerator.MoveNextAsync();

                            var logger = Loggers.GetValue(
                                environment,
                                environment => GetLoggingFunction(environment));

                            logger(serializedQuery, requestId);

                            if (!await moveNext)
                                yield break;
                        }
                        catch (Exception ex)
                        {
                            environment.Logger.LogError($"Error executing Gremlin query with id { requestId }: {ex}");

                            throw;
                        }

                        do
                        {
                            yield return enumerator.Current;
                        } while (await enumerator.MoveNextAsync());
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

        private sealed class WebSocketGremlinQueryExecutor : IGremlinQueryExecutor, IDisposable
        {
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

            public IWebSocketConfigurator ConfigureGremlinServer(Func<GremlinServer, GremlinServer> transformation) => new WebSocketConfigurator(transformation(_gremlinServer), _clientFactory, _alias);

            public IWebSocketConfigurator ConfigureGremlinClientFactory(Func<IGremlinClientFactory, IGremlinClientFactory> transformation) => new WebSocketConfigurator(_gremlinServer, transformation(_clientFactory), _alias);

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

                return new LoggingGremlinQueryExecutor(
                    new WebSocketGremlinQueryExecutor(
                        async ct => await Task.Run(
                            () => _clientFactory.Create(
                                _gremlinServer,
                                JsonNetMessageSerializer.GraphSON3,
                                null,
                                null,
                                null),
                            ct),
                        _alias));
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
                .Transform(source.ConfigureEnvironment(_ => _))
                .ConfigureEnvironment(environment => environment
                    .ConfigureDeserializer(d => d
                        .ConfigureFragmentDeserializer(f => f
                            .AddNewtonsoftJson())));
        }
    }
}
