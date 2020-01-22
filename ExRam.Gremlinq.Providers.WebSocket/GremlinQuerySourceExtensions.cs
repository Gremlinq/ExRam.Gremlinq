using System;
using System.Collections;
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

        public static IGremlinQuerySource UseWebSocket(
            this IGremlinQuerySource source,
            Uri uri,
            GraphsonVersion graphsonVersion,
            string? username = null,
            string? password = null,
            string alias = "g",
            IReadOnlyDictionary<Type, IGraphSONSerializer>? additionalGraphsonSerializers = null,
            IReadOnlyDictionary<string, IGraphSONDeserializer>? additionalGraphsonDeserializers = null)
        {
            return source
                .ConfigureEnvironment(environment => environment
                    .ConfigureExecutionPipeline(conf => conf
                        .UseWebSocketExecutor(uri, username, password, alias, graphsonVersion, additionalGraphsonSerializers, additionalGraphsonDeserializers, environment.Logger)
                        .UseDeserializer(GremlinQueryExecutionResultDeserializer.Graphson)));
        }

        public static IGremlinQueryExecutionPipeline UseWebSocketExecutor(
            this IGremlinQueryExecutionPipeline pipeline,
            Uri uri,
            string? username = null,
            string? password = null,
            string alias = "g",
            GraphsonVersion graphsonVersion = GraphsonVersion.V2,
            IReadOnlyDictionary<Type, IGraphSONSerializer>? additionalGraphsonSerializers = null,
            IReadOnlyDictionary<string, IGraphSONDeserializer>? additionalGraphsonDeserializers = null,
            ILogger? logger = null)
        {
            var actualAdditionalGraphsonSerializers = additionalGraphsonSerializers ?? ImmutableDictionary<Type, IGraphSONSerializer>.Empty;
            var actualAdditionalGraphsonDeserializers = additionalGraphsonDeserializers ?? ImmutableDictionary<string, IGraphSONDeserializer>.Empty;

            if (!"ws".Equals(uri.Scheme, StringComparison.OrdinalIgnoreCase) && !"wss".Equals(uri.Scheme, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException();

            return pipeline
                .UseWebSocketExecutor(
                    () => new GremlinClient(
                        new GremlinServer((uri.Host + uri.AbsolutePath).TrimEnd('/'), uri.Port, "wss".Equals(uri.Scheme, StringComparison.OrdinalIgnoreCase), username, password),
                        graphsonVersion == GraphsonVersion.V2
                            ? new GraphSON2Reader(actualAdditionalGraphsonDeserializers)
                            : (GraphSONReader)new GraphSON3Reader(actualAdditionalGraphsonDeserializers),
                        graphsonVersion == GraphsonVersion.V2
                            ? new GraphSON2Writer(actualAdditionalGraphsonSerializers)
                            : (GraphSONWriter)new GraphSON3Writer(actualAdditionalGraphsonSerializers),
                        graphsonVersion == GraphsonVersion.V2
                            ? GremlinClient.GraphSON2MimeType
                            : GremlinClient.DefaultMimeType),
                    alias,
                    logger);
        }

        public static IGremlinQueryExecutionPipeline UseWebSocketExecutor(this IGremlinQueryExecutionPipeline pipeline, Func<IGremlinClient> clientFactory, string alias = "g", ILogger? logger = null)
        {
            return pipeline
                .UseExecutor(new WebSocketGremlinQueryExecutor(clientFactory, alias, logger));
        }
    }
}
