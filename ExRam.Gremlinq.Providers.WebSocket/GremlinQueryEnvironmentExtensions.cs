using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
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
            private readonly ILogger? _logger;
            private readonly QueryLoggingOptions _loggingOptions;
            private readonly Dictionary<string, string> _aliasArgs;
            private readonly Lazy<IGremlinClient> _lazyGremlinClient;

            public WebSocketGremlinQueryExecutor(
                Func<IGremlinClient> clientFactory,
                string alias = "g",
                ILogger? logger = null,
                QueryLoggingOptions loggingOptions = default)
            {
                _alias = alias;
                _logger = logger;
                _loggingOptions = loggingOptions;
                _aliasArgs = new Dictionary<string, string> { {"g", _alias} };
                _lazyGremlinClient = new Lazy<IGremlinClient>(clientFactory, LazyThreadSafetyMode.ExecutionAndPublication);
            }

            public void Dispose()
            {
                _lazyGremlinClient.Value.Dispose();
            }

            public IAsyncEnumerable<object> Execute(object serializedQuery)
            {
                return AsyncEnumerable.Create(Core);

                async IAsyncEnumerator<object> Core(CancellationToken ct)
                {
                    var results = default(ResultSet<JToken>);

                    if (serializedQuery is GroovyScript groovyScript)
                    {
                        Log(groovyScript);

                        try
                        {
                            results = await _lazyGremlinClient
                                .Value
                                .SubmitAsync<JToken>($"{_alias}.{groovyScript.QueryString}", groovyScript.Bindings)
                                .ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogError(
                                "Error executing Gremlin query {0}:\r\n{1}",
                                groovyScript.QueryString,
                                ex);

                            throw;
                        }
                    }
                    else if (serializedQuery is Bytecode bytecode)
                    {
                        if ((_logger?.IsEnabled(_loggingOptions.LogLevel)).GetValueOrDefault() && _loggingOptions.Verbosity > QueryLoggingVerbosity.None)
                            Log(bytecode.ToGroovy());

                        var requestMsg = RequestMessage.Build(Tokens.OpsBytecode)
                            .Processor(Tokens.ProcessorTraversal)
                            .OverrideRequestId(Guid.NewGuid())
                            .AddArgument(Tokens.ArgsGremlin, bytecode)
                            .AddArgument(Tokens.ArgsAliases, _aliasArgs)
                            .Create();

                        try
                        {
                            results = await _lazyGremlinClient
                                .Value
                                .SubmitAsync<JToken>(requestMsg)
                                .ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogError(
                                "Error executing Gremlin query {0}:\r\n{1}",
                                JsonConvert.SerializeObject(requestMsg),
                                ex);

                            throw;
                        }
                    }
                    else
                        throw new ArgumentException($"Cannot handle serialized query of type {serializedQuery.GetType()}.");

                    if (results != null)
                    {
                        foreach (var obj in results)
                        {
                            yield return obj;
                        }
                    }
                }
            }

            private void Log(GroovyScript query)
            {
                if ((_logger?.IsEnabled(_loggingOptions.LogLevel)).GetValueOrDefault() && _loggingOptions.Verbosity > QueryLoggingVerbosity.None)
                {
                    _logger?.Log(
                        _loggingOptions.LogLevel,
                        "Executing Gremlin query {0}.",
                        JsonConvert.SerializeObject(
                            new
                            {
                                Script = query.QueryString,
                                Bindings = _loggingOptions.Verbosity == QueryLoggingVerbosity.QueryAndParameters
                                    ? query.Bindings
                                    : null
                            }));
                }
            }
        }

        private sealed class WebSocketConfigurationBuilderImpl : IWebSocketConfigurationBuilder
        {
            private readonly Uri? _uri;
            private readonly string _alias;
            private readonly GraphsonVersion _version;
            private readonly IGremlinQueryEnvironment _environment;
            private readonly ConnectionPoolSettings _connectionPoolSettings;
            private readonly QueryLoggingOptions _queryLoggingOptions;
            private readonly (string username, string password)? _auth;
            private readonly ImmutableDictionary<Type, IGraphSONSerializer> _additionalSerializers;
            private readonly ImmutableDictionary<string, IGraphSONDeserializer> _additionalDeserializers;

            public WebSocketConfigurationBuilderImpl(
                IGremlinQueryEnvironment environment,
                Uri? uri,
                GraphsonVersion version,
                (string username, string password)? auth,
                string @alias,
                ImmutableDictionary<Type, IGraphSONSerializer> additionalSerializers,
                ImmutableDictionary<string, IGraphSONDeserializer> additionalDeserializers,
                QueryLoggingOptions queryLoggingOptions, ConnectionPoolSettings connectionPoolSettings)
            {
                _uri = uri;
                _auth = auth;
                _alias = alias;
                _version = version;
                _environment = environment;
                _queryLoggingOptions = queryLoggingOptions;
                _additionalSerializers = additionalSerializers;
                _additionalDeserializers = additionalDeserializers;
                _connectionPoolSettings = connectionPoolSettings;
            }

            public IWebSocketConfigurationBuilder At(Uri uri)
            {
                return new WebSocketConfigurationBuilderImpl(_environment, uri, _version, _auth, _alias, _additionalSerializers, _additionalDeserializers, _queryLoggingOptions, _connectionPoolSettings);
            }

            public IWebSocketConfigurationBuilder SetGraphSONVersion(GraphsonVersion version)
            {
                return new WebSocketConfigurationBuilderImpl(_environment, _uri, version, _auth, _alias, _additionalSerializers, _additionalDeserializers, _queryLoggingOptions, _connectionPoolSettings);
            }

            public IWebSocketConfigurationBuilder AuthenticateBy(string username, string password)
            {
                return new WebSocketConfigurationBuilderImpl(_environment, _uri, _version, (username, password), _alias, _additionalSerializers, _additionalDeserializers, _queryLoggingOptions, _connectionPoolSettings);
            }

            public IWebSocketConfigurationBuilder SetAlias(string alias)
            {
                return new WebSocketConfigurationBuilderImpl(_environment, _uri, _version, _auth, alias, _additionalSerializers, _additionalDeserializers, _queryLoggingOptions, _connectionPoolSettings);
            }

            public IWebSocketConfigurationBuilder ConfigureConnectionPool(Action<ConnectionPoolSettings> transformation)
            {
                var newConnectionPoolSettings = new ConnectionPoolSettings
                {
                    MaxInProcessPerConnection = _connectionPoolSettings.MaxInProcessPerConnection,
                    PoolSize = _connectionPoolSettings.PoolSize
                };

                transformation(newConnectionPoolSettings);

                return new WebSocketConfigurationBuilderImpl(_environment, _uri, _version, _auth, _alias, _additionalSerializers, _additionalDeserializers, _queryLoggingOptions, newConnectionPoolSettings);
            }

            public IWebSocketConfigurationBuilder AddGraphSONSerializer(Type type, IGraphSONSerializer serializer)
            {
                return new WebSocketConfigurationBuilderImpl(_environment, _uri, _version, _auth, _alias, _additionalSerializers.SetItem(type, serializer), _additionalDeserializers, _queryLoggingOptions, _connectionPoolSettings);
            }

            public IWebSocketConfigurationBuilder AddGraphSONDeserializer(string typename, IGraphSONDeserializer deserializer)
            {
                return new WebSocketConfigurationBuilderImpl(_environment, _uri, _version, _auth, _alias, _additionalSerializers, _additionalDeserializers.SetItem(typename, deserializer), _queryLoggingOptions, _connectionPoolSettings);
            }

            public IWebSocketConfigurationBuilder ConfigureQueryLoggingOptions(Func<QueryLoggingOptions, QueryLoggingOptions> transformation)
            {
                return new WebSocketConfigurationBuilderImpl(_environment, _uri, _version, _auth, _alias, _additionalSerializers, _additionalDeserializers, transformation(_queryLoggingOptions), _connectionPoolSettings);
            }

            public IGremlinQueryEnvironment Build()
            {
                if (_uri == null)
                    throw new InvalidOperationException($"No valid Gremlin endpoint found. Configure {nameof(GremlinQuerySource.g)} with {nameof(UseWebSocket)} and use {nameof(At)} on the builder to set a valid Gremlin endpoint.");

                if (!"ws".Equals(_uri.Scheme, StringComparison.OrdinalIgnoreCase) && !"wss".Equals(_uri.Scheme, StringComparison.OrdinalIgnoreCase))
                    throw new ArgumentException("Expected the Uri-Scheme to be either \"ws\" or \"wss\".");

                return _environment
                    .UseExecutor(
                        new WebSocketGremlinQueryExecutor(
                            () => new GremlinClient(
                                new GremlinServer((_uri.Host + _uri.AbsolutePath).TrimEnd('/'), _uri.Port, "wss".Equals(_uri.Scheme, StringComparison.OrdinalIgnoreCase), _auth?.username, _auth?.password),
                                _version == GraphsonVersion.V2
                                    ? new GraphSON2Reader(_additionalDeserializers)
                                    : (GraphSONReader)new GraphSON3Reader(_additionalDeserializers),
                                _version == GraphsonVersion.V2
                                    ? new GraphSON2Writer(_additionalSerializers)
                                    : (GraphSONWriter)new GraphSON3Writer(_additionalSerializers),
                                _version == GraphsonVersion.V2
                                    ? GremlinClient.GraphSON2MimeType
                                    : GremlinClient.DefaultMimeType,
                                _connectionPoolSettings),
                            _alias,
                            _environment.Logger,
                            _queryLoggingOptions));
            }
        }

        public static IGremlinQueryEnvironment UseWebSocket(
            this IGremlinQueryEnvironment environment,
            Func<IWebSocketConfigurationBuilder, IGremlinQueryEnvironmentBuilder> builderAction)
        {
            return builderAction(new WebSocketConfigurationBuilderImpl(environment, default, GraphsonVersion.V3, null, "g", ImmutableDictionary<Type, IGraphSONSerializer>.Empty, ImmutableDictionary<string, IGraphSONDeserializer>.Empty, QueryLoggingOptions.Default, new ConnectionPoolSettings()))
                .Build()
                .UseDeserializer(GremlinQueryExecutionResultDeserializer.Graphson);
        }
    }
}
