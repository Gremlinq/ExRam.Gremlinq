using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using ExRam.Gremlinq.Core;
using Gremlin.Net.Driver;
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
            private readonly ILogger? _logger;
            private readonly Lazy<IGremlinClient> _lazyGremlinClient;

            public WebSocketGremlinQueryExecutor(
                Func<IGremlinClient> clientFactory,
                ILogger? logger = null)
            {
                _logger = logger;
                _lazyGremlinClient = new Lazy<IGremlinClient>(clientFactory, LazyThreadSafetyMode.ExecutionAndPublication);
            }

            public void Dispose()
            {
                _lazyGremlinClient.Value.Dispose();
            }

            public IAsyncEnumerable<object> Execute(object serializedQuery)
            {
                /*if (serializedQuery is Bytecode groovySerializedQuery)
                {
                    _logger?.LogTrace("Executing Gremlin query {0}.", groovySerializedQuery.QueryString);

                    return _lazyGremlinClient
                        .Value
                        .SubmitAsync<JToken>(groovySerializedQuery.QueryString, groovySerializedQuery.Bindings)
                        .ToAsyncEnumerable()
                        .SelectMany(x => x
                            .ToAsyncEnumerable())
                        .Catch<JToken, Exception>(ex =>
                        {
                            _logger?.LogError("Error executing Gremlin query {0}.", groovySerializedQuery.QueryString);

                            return AsyncEnumerableEx.Throw<JToken>(ex);
                        });
                }*/

                throw new ArgumentException($"Cannot handle serialized query of type {serializedQuery.GetType()}.");
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
            IReadOnlyDictionary<Type, IGraphSONSerializer>? additionalGraphsonSerializers = null,
            IReadOnlyDictionary<string, IGraphSONDeserializer>? additionalGraphsonDeserializers = null)
        {
            return source.ConfigureExecutionPipeline(conf => conf
                .UseSerializer(GremlinQuerySerializer.Default)
                .UseWebSocketExecutor(hostname, port, enableSsl, username, password, graphsonVersion, additionalGraphsonSerializers, additionalGraphsonDeserializers, source.Logger)
                .UseDeserializer(GremlinQueryExecutionResultDeserializer.Graphson));
        }

        public static IGremlinQueryExecutionPipeline UseWebSocketExecutor(
            this IGremlinQueryExecutionPipeline pipeline,
            string hostname,
            int port = 8182,
            bool enableSsl = false,
            string? username = null,
            string? password = null,
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
                    logger);
        }

        public static IGremlinQueryExecutionPipeline UseWebSocketExecutor(this IGremlinQueryExecutionPipeline pipeline, Func<IGremlinClient> clientFactory, ILogger? logger = null)
        {
            return pipeline
                .UseExecutor(new WebSocketGremlinQueryExecutor(clientFactory, logger));
        }
    }
}
