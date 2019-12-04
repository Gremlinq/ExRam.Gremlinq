using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;
using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;
using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Structure.IO.GraphSON;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Providers.WebSocket
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
                Task<ResultSet<JToken>> submitTask;

                if (serializedQuery is GroovySerializedGremlinQuery groovyScript)
                {
                    _logger?.LogTrace("Executing Gremlin query {0}.", groovyScript.QueryString);

                    submitTask = _lazyGremlinClient
                        .Value
                        .SubmitAsync<JToken>(groovyScript.QueryString, groovyScript.Bindings);
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

                    submitTask = _lazyGremlinClient
                        .Value
                        .SubmitAsync<JToken>(requestMsg);
                }
                else
                    throw new ArgumentException($"Cannot handle serialized query of type {serializedQuery.GetType()}.");

                return submitTask
                    .ToAsyncEnumerable()
                    .SelectMany(x => x
                        .ToAsyncEnumerable())
                    .Catch<JToken, Exception>(ex =>
                    {
                        _logger?.LogError("Error executing Gremlin query {0}.", groovyScript.QueryString);

                        return AsyncEnumerableEx.Throw<JToken>(ex);
                    });
            }
        }

        public static IConfigurableGremlinQuerySource UseWebSocket(
            this IConfigurableGremlinQuerySource source,
            string hostname,
            GraphsonVersion graphsonVersion,
            int port = 8182,
            bool enableSsl = false,
            string? username = null,
            string? password = null,
            string alias = "g",
            IReadOnlyDictionary<Type, IGraphSONSerializer>? additionalGraphsonSerializers = null,
            IReadOnlyDictionary<string, IGraphSONDeserializer>? additionalGraphsonDeserializers = null)
        {
            return source.ConfigureExecutionPipeline(conf => conf
                .UseWebSocketExecutor(hostname, port, enableSsl, username, password, alias, graphsonVersion, additionalGraphsonSerializers, additionalGraphsonDeserializers, source.Logger)
                .UseDeserializer(GremlinQueryExecutionResultDeserializer.Graphson));
        }

        public static IGremlinQueryExecutionPipeline UseWebSocketExecutor(
            this IGremlinQueryExecutionPipeline pipeline,
            string hostname,
            int port = 8182,
            bool enableSsl = false,
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

            return pipeline
                .UseWebSocketExecutor(
                    () => new GremlinClient(
                        new GremlinServer(hostname, port, enableSsl, username, password),
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
