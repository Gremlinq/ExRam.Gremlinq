using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
    public static class GremlinQueryEnvironmentExtensions
    {
        private sealed class WebSocketGremlinQueryExecutor : IGremlinQueryExecutor, IDisposable
        {
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

                    var requestMessage = serializedQuery switch
                    {
                        GroovyGremlinQuery groovyScript => RequestMessage
                            .Build(Tokens.OpsEval)
                            .AddArgument(Tokens.ArgsGremlin, $"{_alias}.{groovyScript.Script}")
                            .AddArgument(Tokens.ArgsBindings, groovyScript.Bindings)
                            .Create(),
                        Bytecode bytecode => RequestMessage.Build(Tokens.OpsBytecode)
                            .Processor(Tokens.ProcessorTraversal)
                            .AddArgument(Tokens.ArgsGremlin, bytecode)
                            .AddArgument(Tokens.ArgsAliases, _aliasArgs)
                            .Create(),
                        _ => throw new ArgumentException($"Cannot handle serialized query of type {serializedQuery.GetType()}.")
                    };

                    Log(requestMessage, requestMessage.RequestId, environment);

                    try
                    {
                        var client = await _lazyGremlinClient
                            .Value;

                        results = await client
                            .SubmitAsync<JToken>(requestMessage)
                            .ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        environment.Logger.LogError(
                            $"Error executing Gremlin query with {nameof(RequestMessage.RequestId)} {0}: {1}",
                            requestMessage.RequestId,
                            ex);

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

            private void Log(object serializedQuery, Guid requestId, IGremlinQueryEnvironment environment)
            {
                var logLevel = environment.Options.GetValue(GremlinqOption.QueryLogLogLevel);
                var verbosity = environment.Options.GetValue(GremlinqOption.QueryLogVerbosity);

                if (environment.Logger.IsEnabled(logLevel))
                {
                    var gremlinQuery = serializedQuery switch
                    {
                        Bytecode bytecode => bytecode.ToGroovy(),
                        GroovyGremlinQuery groovyGremlinQuery => groovyGremlinQuery,
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
                                Bindings = (verbosity & QueryLogVerbosity.IncludeParameters) >
                                           QueryLogVerbosity.QueryOnly
                                    ? gremlinQuery.Bindings
                                    : null
                            }));
                }
            }
        }

        private sealed class WebSocketGremlinQueryExecutorBuilderImpl : IWebSocketGremlinQueryExecutorBuilder
        {
            private readonly Uri? _uri;
            private readonly string _alias;
            private readonly SerializationFormat _format;
            private readonly ConnectionPoolSettings _connectionPoolSettings;
            private readonly (string username, string password)? _auth;
            private readonly Func<IGremlinClient, IGremlinClient> _clientTransformation;
            private readonly ImmutableDictionary<Type, IGraphSONSerializer> _additionalSerializers;
            private readonly ImmutableDictionary<string, IGraphSONDeserializer> _additionalDeserializers;

            public WebSocketGremlinQueryExecutorBuilderImpl(
                IGremlinQueryEnvironment environment,
                Uri? uri,
                SerializationFormat format,
                (string username, string password)? auth,
                string @alias,
                Func<IGremlinClient, IGremlinClient> clientTransformation,
                ImmutableDictionary<Type, IGraphSONSerializer> additionalSerializers,
                ImmutableDictionary<string, IGraphSONDeserializer> additionalDeserializers,
                ConnectionPoolSettings connectionPoolSettings)
            {
                _uri = uri;
                _auth = auth;
                _alias = alias;
                _format = format;
                Environment = environment;
                _additionalSerializers = additionalSerializers;
                _additionalDeserializers = additionalDeserializers;
                _connectionPoolSettings = connectionPoolSettings;
                _clientTransformation = clientTransformation;
            }

            public IWebSocketGremlinQueryExecutorBuilder At(Uri uri)
            {
                return new WebSocketGremlinQueryExecutorBuilderImpl(Environment, uri, _format, _auth, _alias, _clientTransformation, _additionalSerializers, _additionalDeserializers, _connectionPoolSettings);
            }

            public IWebSocketGremlinQueryExecutorBuilder ConfigureGremlinClient(Func<IGremlinClient, IGremlinClient> transformation)
            {
                return new WebSocketGremlinQueryExecutorBuilderImpl(Environment, _uri, _format, _auth, _alias, _ => transformation(_clientTransformation(_)), _additionalSerializers, _additionalDeserializers, _connectionPoolSettings);
            }

            public IWebSocketGremlinQueryExecutorBuilder SetSerializationFormat(SerializationFormat version)
            {
                return new WebSocketGremlinQueryExecutorBuilderImpl(Environment, _uri, version, _auth, _alias, _clientTransformation, _additionalSerializers, _additionalDeserializers, _connectionPoolSettings);
            }

            public IWebSocketGremlinQueryExecutorBuilder AuthenticateBy(string username, string password)
            {
                return new WebSocketGremlinQueryExecutorBuilderImpl(Environment, _uri, _format, (username, password), _alias, _clientTransformation, _additionalSerializers, _additionalDeserializers, _connectionPoolSettings);
            }

            public IWebSocketGremlinQueryExecutorBuilder SetAlias(string alias)
            {
                return new WebSocketGremlinQueryExecutorBuilderImpl(Environment, _uri, _format, _auth, alias, _clientTransformation, _additionalSerializers, _additionalDeserializers, _connectionPoolSettings);
            }

            public IWebSocketGremlinQueryExecutorBuilder ConfigureConnectionPool(Action<ConnectionPoolSettings> transformation)
            {
                var newConnectionPoolSettings = new ConnectionPoolSettings
                {
                    MaxInProcessPerConnection = _connectionPoolSettings.MaxInProcessPerConnection,
                    PoolSize = _connectionPoolSettings.PoolSize
                };

                transformation(newConnectionPoolSettings);

                return new WebSocketGremlinQueryExecutorBuilderImpl(Environment, _uri, _format, _auth, _alias, _clientTransformation, _additionalSerializers, _additionalDeserializers, newConnectionPoolSettings);
            }

            public IWebSocketGremlinQueryExecutorBuilder AddGraphSONSerializer(Type type, IGraphSONSerializer serializer)
            {
                return new WebSocketGremlinQueryExecutorBuilderImpl(Environment, _uri, _format, _auth, _alias, _clientTransformation, _additionalSerializers.SetItem(type, serializer), _additionalDeserializers, _connectionPoolSettings);
            }

            public IWebSocketGremlinQueryExecutorBuilder AddGraphSONDeserializer(string typename, IGraphSONDeserializer deserializer)
            {
                return new WebSocketGremlinQueryExecutorBuilderImpl(Environment, _uri, _format, _auth, _alias, _clientTransformation, _additionalSerializers, _additionalDeserializers.SetItem(typename, deserializer), _connectionPoolSettings);
            }

            public IGremlinQueryExecutor Build()
            {
                if (_uri == null)
                    throw new InvalidOperationException(
                        $"No valid Gremlin endpoint found. Configure {nameof(GremlinQuerySource.g)} with {nameof(UseWebSocket)} and use {nameof(At)} on the builder to set a valid Gremlin endpoint.");

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
                                ? new GraphSON2Reader(_additionalDeserializers)
                                : (GraphSONReader)new GraphSON3Reader(_additionalDeserializers),
                            _format == SerializationFormat.GraphSonV2
                                ? new GraphSON2Writer(_additionalSerializers)
                                : (GraphSONWriter)new GraphSON3Writer(_additionalSerializers),
                            _format == SerializationFormat.GraphSonV2
                                ? GremlinClient.GraphSON2MimeType
                                : GremlinClient.DefaultMimeType,
                            _connectionPoolSettings),
                        ct)),
                    _alias);
            }

            public IGremlinQueryEnvironment Environment { get; }
        }

        public static IGremlinQueryEnvironment UseWebSocket(
            this IGremlinQueryEnvironment environment,
            Func<IWebSocketGremlinQueryExecutorBuilder, IGremlinQueryExecutorBuilder> builderTransformation)
        {
            var builder = new WebSocketGremlinQueryExecutorBuilderImpl(
                environment,
                default,
                SerializationFormat.GraphSonV3,
                null,
                "g",
                _ => _,
                ImmutableDictionary<Type, IGraphSONSerializer>.Empty,
                ImmutableDictionary<string, IGraphSONDeserializer>.Empty, new ConnectionPoolSettings());

            return environment
                .UseExecutor(builderTransformation(builder).Build())
                .UseDeserializer(GremlinQueryExecutionResultDeserializer.FromJToken);
        }
    }
}
