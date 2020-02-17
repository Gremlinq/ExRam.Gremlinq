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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core
{
    public static class WebSocketQuerySourceBuilder
    {
        public static IWebSocketQuerySourceBuilder AtLocalhost(this IWebSocketQuerySourceBuilder builder)
        {
            return builder.At(new Uri("ws://localhost:8182"));
        }
    }

    public static class GremlinQueryEnvironmentExtensions
    {
        private sealed class WebSocketGremlinQueryExecutor : IGremlinQueryExecutor, IDisposable
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
            private readonly Uri? _uri;
            private readonly string _alias;
            private readonly GraphsonVersion _version;
            private readonly IGremlinQueryEnvironment _environment;
            private readonly (string username, string password)? _auth;
            private readonly ImmutableDictionary<Type, IGraphSONSerializer> _additionalSerializers;
            private readonly ImmutableDictionary<string, IGraphSONDeserializer> _additionalDeserializers;

            public WebSocketQuerySourceBuilderImpl(
                IGremlinQueryEnvironment environment,
                Uri? uri,
                GraphsonVersion version,
                (string username, string password)? auth,
                string @alias,
                ImmutableDictionary<Type, IGraphSONSerializer> additionalSerializers,
                ImmutableDictionary<string, IGraphSONDeserializer> additionalDeserializers)
            {
                _uri = uri;
                _auth = auth;
                _alias = alias;
                _version = version;
                _environment = environment;
                _additionalSerializers = additionalSerializers;
                _additionalDeserializers = additionalDeserializers;
            }

            public IWebSocketQuerySourceBuilder At(Uri uri)
            {
                return new WebSocketQuerySourceBuilderImpl(_environment, uri, _version, _auth, _alias, _additionalSerializers, _additionalDeserializers);
            }

            public IWebSocketQuerySourceBuilder SetGraphSONVersion(GraphsonVersion version)
            {
                return new WebSocketQuerySourceBuilderImpl(_environment, _uri, version, _auth, _alias, _additionalSerializers, _additionalDeserializers);
            }

            public IWebSocketQuerySourceBuilder AuthenticateBy(string username, string password)
            {
                return new WebSocketQuerySourceBuilderImpl(_environment, _uri, _version, (username, password), _alias, _additionalSerializers, _additionalDeserializers);
            }

            public IWebSocketQuerySourceBuilder SetAlias(string alias)
            {
                return new WebSocketQuerySourceBuilderImpl(_environment, _uri, _version, _auth, alias, _additionalSerializers, _additionalDeserializers);
            }

            public IWebSocketQuerySourceBuilder AddGraphSONSerializer(Type type, IGraphSONSerializer serializer)
            {
                return new WebSocketQuerySourceBuilderImpl(_environment, _uri, _version, _auth, _alias, _additionalSerializers.SetItem(type, serializer), _additionalDeserializers);
            }

            public IWebSocketQuerySourceBuilder AddGraphSONDeserializer(string typename, IGraphSONDeserializer deserializer)
            {
                return new WebSocketQuerySourceBuilderImpl(_environment, _uri, _version, _auth, _alias, _additionalSerializers, _additionalDeserializers.SetItem(typename, deserializer));
            }

            public IGremlinQueryEnvironment Build()
            {
                if (_uri == null)
                    throw new InvalidOperationException($"No valid Gremlin endpoint found. Configure {nameof(GremlinQuerySource.g)} with {nameof(UseWebSocket)} and use {nameof(At)} on the builder to set a valid Gremlin endpoint.");

                if (!"ws".Equals(_uri.Scheme, StringComparison.OrdinalIgnoreCase) && !"wss".Equals(_uri.Scheme, StringComparison.OrdinalIgnoreCase))
                    throw new ArgumentException();

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
                                    : GremlinClient.DefaultMimeType),
                            _alias,
                            _environment.Logger));
            }
        }

        public static IGremlinQueryEnvironment UseWebSocket(
            this IGremlinQueryEnvironment environment,
            Func<IWebSocketQuerySourceBuilder, IWebSocketQuerySourceBuilder> builderAction)
        {
            return builderAction(new WebSocketQuerySourceBuilderImpl(environment, default, GraphsonVersion.V3, null, "g", ImmutableDictionary<Type, IGraphSONSerializer>.Empty, ImmutableDictionary<string, IGraphSONDeserializer>.Empty))
                .Build()
                .UseDeserializer(GremlinQueryExecutionResultDeserializer.Graphson);
        }
    }
}
