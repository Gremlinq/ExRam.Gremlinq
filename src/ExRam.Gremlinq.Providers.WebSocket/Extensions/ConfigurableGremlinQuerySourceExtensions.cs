using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core.Deserialization;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Providers.WebSocket;
using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;
using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Structure.IO.GraphSON;
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
            private readonly Lazy<Task<IGremlinClient>> _lazyGremlinClient;

            public WebSocketGremlinQueryExecutor(
                Func<CancellationToken, Task<IGremlinClient>> clientFactory,
                string alias = "g")
            {
                _alias = alias;
                _aliasArgs = new Dictionary<string, string> { {"g", _alias} };
                _lazyGremlinClient = new Lazy<Task<IGremlinClient>>(() => clientFactory(default), LazyThreadSafetyMode.ExecutionAndPublication);
            }

            public void Dispose()
            {
                _lazyGremlinClient.Value.Dispose();
            }

            public IAsyncEnumerable<object> Execute(object serializedQuery, IGremlinQueryEnvironment environment)
            {
                return AsyncEnumerable.Create(Core);

                async IAsyncEnumerator<object> Core(CancellationToken ct)
                {
                    var results = default(ResultSet<JToken>);
                    var client = await _lazyGremlinClient.Value;

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
            private abstract class JsonNetMessageSerializer : IMessageSerializer
            {
                public sealed class GraphSON2 : JsonNetMessageSerializer
                {
                    public GraphSON2() : base("application/vnd.gremlin-v2.0+json", new GraphSON2Writer())
                    {

                    }
                }

                public sealed class GraphSON3 : JsonNetMessageSerializer
                {
                    public GraphSON3() : base("application/vnd.gremlin-v3.0+json", new GraphSON3Writer())
                    {

                    }
                }

                private static readonly JsonSerializer Serializer = JsonSerializer.CreateDefault();

                private readonly string _mimeType;
                private readonly GraphSONWriter _graphSONWriter;

                protected JsonNetMessageSerializer(string mimeType, GraphSONWriter graphSonWriter)
                {
                    _mimeType = mimeType;
                    _graphSONWriter = graphSonWriter;
                }

                public async Task<byte[]> SerializeMessageAsync(RequestMessage requestMessage)
                {
                    var graphSONMessage = _graphSONWriter.WriteObject(requestMessage);
                    return Encoding.UTF8.GetBytes(MessageWithHeader(graphSONMessage));
                }

                private string MessageWithHeader(string messageContent)
                {
                    return $"{(char)_mimeType.Length}{_mimeType}{messageContent}";
                }

                public async Task<ResponseMessage<List<object>>> DeserializeMessageAsync(byte[] message)
                {
                    if (message.Length == 0)
                        return null!;

                    var responseMessage = Serializer
                        .Deserialize<ResponseMessage<JToken>>(new JsonTextReader(new StreamReader(new MemoryStream(message))));

                    return new ResponseMessage<List<object>>
                    {
                        RequestId = responseMessage.RequestId,
                        Status = responseMessage.Status,
                        Result = new ResponseResult<List<object>>
                        {
                            Data = new List<object>
                            {
                                responseMessage.Result.Data
                            },
                            Meta = responseMessage.Result.Meta
                        }
                    };
                }
            }

            private readonly Uri? _uri;
            private readonly string _alias;
            private readonly SerializationFormat _format;
            private readonly ConnectionPoolSettings _connectionPoolSettings;
            private readonly (string username, string password)? _auth;
            private readonly Func<IGremlinClient, IGremlinClient> _clientTransformation;

            public WebSocketConfigurator(
                Uri? uri,
                SerializationFormat format,
                (string username, string password)? auth,
                string alias,
                Func<IGremlinClient, IGremlinClient> clientTransformation,
                ConnectionPoolSettings connectionPoolSettings)
            {
                _uri = uri;
                _auth = auth;
                _alias = alias;
                _format = format;
                _clientTransformation = clientTransformation;
                _connectionPoolSettings = connectionPoolSettings;
            }

            public IWebSocketConfigurator At(Uri uri) => new WebSocketConfigurator(uri, _format, _auth, _alias, _clientTransformation, _connectionPoolSettings);

            public IWebSocketConfigurator ConfigureGremlinClient(Func<IGremlinClient, IGremlinClient> transformation) => new WebSocketConfigurator(_uri, _format, _auth, _alias, _ => transformation(_clientTransformation(_)), _connectionPoolSettings);

            public IWebSocketConfigurator SetSerializationFormat(SerializationFormat version) => new WebSocketConfigurator(_uri, version, _auth, _alias, _clientTransformation, _connectionPoolSettings);

            public IWebSocketConfigurator AuthenticateBy(string username, string password) => new WebSocketConfigurator(_uri, _format, (username, password), _alias, _clientTransformation, _connectionPoolSettings);

            public IWebSocketConfigurator SetAlias(string alias) => new WebSocketConfigurator(_uri, _format, _auth, alias, _clientTransformation, _connectionPoolSettings);

            public IWebSocketConfigurator ConfigureConnectionPool(Action<ConnectionPoolSettings> transformation)
            {
                var newConnectionPoolSettings = new ConnectionPoolSettings
                {
                    MaxInProcessPerConnection = _connectionPoolSettings.MaxInProcessPerConnection,
                    PoolSize = _connectionPoolSettings.PoolSize
                };

                transformation(newConnectionPoolSettings);

                return new WebSocketConfigurator(_uri, _format, _auth, _alias, _clientTransformation, newConnectionPoolSettings);
            }

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
                        () => new GremlinClient(
                            new GremlinServer(
                                (_uri.Host + _uri.AbsolutePath).TrimEnd('/'),
                                _uri.Port,
                                "wss".Equals(_uri.Scheme, StringComparison.OrdinalIgnoreCase),
                                _auth?.username,
                                _auth?.password),
                            _format == SerializationFormat.GraphSonV2
                                ? new JsonNetMessageSerializer.GraphSON2()
                                : new JsonNetMessageSerializer.GraphSON3(),
                            _connectionPoolSettings),
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
                SerializationFormat.GraphSonV3,
                null,
                "g",
                _ => _,
                new ConnectionPoolSettings());

            return configuratorTransformation(configurator)
                .Transform(source.ConfigureEnvironment(_ => _))
                .ConfigureEnvironment(environment => environment
                    .ConfigureDeserializer(d => d
                        .ConfigureFragmentDeserializer(f => f
                            .AddNewtonsoftJson())));
        }
    }
}
