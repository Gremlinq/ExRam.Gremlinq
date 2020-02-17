using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;
using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Structure.IO.GraphSON;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core
{
    public interface IWebSocketQuerySourceBuilder
    {
        IWebSocketQuerySourceBuilder WithUri(Uri uri);

        IWebSocketQuerySourceBuilder WithGraphSONVersion(GraphsonVersion version);

        IWebSocketQuerySourceBuilder WithAuthentication(string username, string password);

        IWebSocketQuerySourceBuilder WithAlias(string alias);

        IWebSocketQuerySourceBuilder WithGraphSONSerializer(Type type, IGraphSONSerializer serializer);

        IWebSocketQuerySourceBuilder WithGraphSONDeserializer(string typename, IGraphSONDeserializer serializer);

        IWebSocketQuerySourceBuilder WithLogger(ILogger logger);

        IGremlinQueryExecutionPipeline Build();
    }

    public static class WebSocketQuerySourceBuilder
    {
        
    }

    public static class GremlinQuerySourceExtensions
    {
        private class WebSocketGremlinQueryExecutor : IGremlinQueryExecutor, IDisposable
        {
            private readonly string _alias;
            private readonly ILogger? _logger;
            private readonly Lazy<IGremlinClient> _lazyGremlinClient;

            public WebSocketGremlinQueryExecutor(
                Func<IGremlinClient> clientFactory,
                string alias = "g",
                ILogger? logger = null)
            {
                _alias = alias;
                _logger = logger;
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

                    if (serializedQuery is GroovySerializedGremlinQuery groovyScript)
                    {
                        _logger?.LogTrace("Executing Gremlin query {0}.", groovyScript.QueryString);

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
                        _logger?.LogTrace("Executing Gremlin query {0}.", bytecode);

                        var requestMsg = RequestMessage.Build(Tokens.OpsBytecode)
                            .Processor(Tokens.ProcessorTraversal)
                            .OverrideRequestId(Guid.NewGuid())
                                .AddArgument(Tokens.ArgsGremlin, bytecode)
                                .AddArgument(Tokens.ArgsAliases, new Dictionary<string, string> { { "g", _alias } })
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
        }

        private sealed class WebSocketQuerySourceBuilderImpl : IWebSocketQuerySourceBuilder
        {
            private readonly Uri _uri;
            private readonly string _alias;
            private readonly ILogger _logger;
            private readonly GraphsonVersion _version;
            private readonly IGremlinQueryExecutionPipeline _pipeline;
            private readonly (string username, string password)? _auth;
            private readonly ImmutableDictionary<Type, IGraphSONSerializer> _additionalSerializers;
            private readonly ImmutableDictionary<string, IGraphSONDeserializer> _additionalDeserializers;

            public WebSocketQuerySourceBuilderImpl(
                IGremlinQueryExecutionPipeline pipeline,
                Uri uri,
                GraphsonVersion version,
                (string username, string password)? auth,
                string @alias,
                ImmutableDictionary<Type, IGraphSONSerializer> additionalSerializers,
                ImmutableDictionary<string, IGraphSONDeserializer> additionalDeserializers, ILogger logger)
            {
                _uri = uri;
                _auth = auth;
                _alias = alias;
                _logger = logger;
                _pipeline = pipeline;
                _version = version;
                _additionalSerializers = additionalSerializers;
                _additionalDeserializers = additionalDeserializers;
            }

            public IWebSocketQuerySourceBuilder WithUri(Uri uri)
            {
                return new WebSocketQuerySourceBuilderImpl(_pipeline, uri, _version, _auth, _alias, _additionalSerializers, _additionalDeserializers, _logger);
            }

            public IWebSocketQuerySourceBuilder WithGraphSONVersion(GraphsonVersion version)
            {
                return new WebSocketQuerySourceBuilderImpl(_pipeline, _uri, version, _auth, _alias, _additionalSerializers, _additionalDeserializers, _logger);
            }

            public IWebSocketQuerySourceBuilder WithAuthentication(string username, string password)
            {
                return new WebSocketQuerySourceBuilderImpl(_pipeline, _uri, _version, (username, password), _alias, _additionalSerializers, _additionalDeserializers, _logger);
            }

            public IWebSocketQuerySourceBuilder WithAlias(string alias)
            {
                return new WebSocketQuerySourceBuilderImpl(_pipeline, _uri, _version, _auth, alias, _additionalSerializers, _additionalDeserializers, _logger);
            }

            public IWebSocketQuerySourceBuilder WithGraphSONSerializer(Type type, IGraphSONSerializer serializer)
            {
                return new WebSocketQuerySourceBuilderImpl(_pipeline, _uri, _version, _auth, _alias, _additionalSerializers.SetItem(type, serializer), _additionalDeserializers, _logger);
            }

            public IWebSocketQuerySourceBuilder WithGraphSONDeserializer(string typename, IGraphSONDeserializer deserializer)
            {
                return new WebSocketQuerySourceBuilderImpl(_pipeline, _uri, _version, _auth, _alias, _additionalSerializers, _additionalDeserializers.SetItem(typename, deserializer), _logger);
            }

            public IWebSocketQuerySourceBuilder WithLogger(ILogger logger)
            {
                return new WebSocketQuerySourceBuilderImpl(_pipeline, _uri, _version, _auth, _alias, _additionalSerializers, _additionalDeserializers, _logger);
            }

            public IGremlinQueryExecutionPipeline Build()
            {
                if (!"ws".Equals(_uri.Scheme, StringComparison.OrdinalIgnoreCase) && !"wss".Equals(_uri.Scheme, StringComparison.OrdinalIgnoreCase))
                    throw new ArgumentException();

                return _pipeline.UseWebSocketExecutor(
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
                            : GremlinClient.DefaultMimeType),
                    _alias,
                    _logger);
            }
        }

        public static IGremlinQuerySource UseWebSocket(
            this IGremlinQuerySource source,
            Action<IWebSocketQuerySourceBuilder> builderAction)
        {
            return source
                .ConfigureEnvironment(environment => environment
                    .ConfigureExecutionPipeline(conf => conf
                        .UseWebSocketExecutor(builder =>
                        {
                            builderAction(builder);
                            builder.WithLogger(environment.Logger);
                        })
                        .UseDeserializer(GremlinQueryExecutionResultDeserializer.Graphson)));
        }

        public static IGremlinQueryExecutionPipeline UseWebSocketExecutor(
            this IGremlinQueryExecutionPipeline pipeline,
            Action<IWebSocketQuerySourceBuilder> builderAction)
        {
            var builder = new WebSocketQuerySourceBuilderImpl(pipeline, new Uri("ws://localhost:8182"), GraphsonVersion.V3, null, "g", ImmutableDictionary<Type, IGraphSONSerializer>.Empty, ImmutableDictionary<string, IGraphSONDeserializer>.Empty, NullLogger.Instance);

            builderAction(builder);

            return builder.Build();
        }

        public static IGremlinQueryExecutionPipeline UseWebSocketExecutor(this IGremlinQueryExecutionPipeline pipeline, Func<IGremlinClient> clientFactory, string alias = "g", ILogger? logger = null)
        {
            return pipeline
                .UseExecutor(new WebSocketGremlinQueryExecutor(clientFactory, alias, logger));
        }
    }
}
