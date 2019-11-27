using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using ExRam.Gremlinq.Core;
using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;
using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Process.Traversal.Strategy;
using Gremlin.Net.Process.Traversal.Strategy.Decoration;
using Gremlin.Net.Structure.IO.GraphSON;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    internal class ReferenceElementStrategy : AbstractTraversalStrategy
    {
        public ReferenceElementStrategy()
        {

        }
    }

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
                if (serializedQuery is GroovySerializedGremlinQuery groovyScript)
                {
                    _logger?.LogTrace("Executing Gremlin query {0}.", groovyScript.QueryString);

                    return _lazyGremlinClient
                        .Value
                        .SubmitAsync<JToken>(groovyScript.QueryString, groovyScript.Bindings)
                        .ToAsyncEnumerable()
                        .SelectMany(x => x
                            .ToAsyncEnumerable())
                        .Catch<JToken, Exception>(ex =>
                        {
                            _logger?.LogError("Error executing Gremlin query {0}.", groovyScript.QueryString);

                            return AsyncEnumerableEx.Throw<JToken>(ex);
                        });
                }

                if (serializedQuery is Bytecode bytecode)
                {
                    //bytecode.SourceInstructions.Add(new Instruction("withoutStrategies", typeof(ReferenceElementStrategy)));

                    _logger?.LogTrace("Executing Gremlin query {0}.", bytecode);

                    var requestId = Guid.NewGuid();

                    var requestMsg = RequestMessage.Build(Tokens.OpsBytecode)
                        .Processor(Tokens.ProcessorTraversal)
                        .OverrideRequestId(requestId)
                            .AddArgument(Tokens.ArgsGremlin, bytecode)
                            .AddArgument(Tokens.ArgsAliases, new Dictionary<string, string> { { "g", "g" } })
                            .Create();

                    return _lazyGremlinClient
                        .Value
                        .SubmitAsync<JToken>(requestMsg)
                        .ToAsyncEnumerable()
                        .SelectMany(x => x
                            .ToAsyncEnumerable())
                        .SelectMany(token =>
                        {
                            if (token["@type"].ToString() == "g:List")
                            {
                                if (token["@value"] is JArray array)
                                    return array.ToAsyncEnumerable();
                            }

                            return AsyncEnumerable.Empty<JToken>();
                        })
                        .Select(listToken =>
                        {
                            if (listToken["@type"].ToString() == "g:Traverser")
                            {
                                if (listToken["@value"] is JObject traverser)
                                    return traverser;
                            }

                            return null;
                        })
                        .Where(x => x != null)
                        .SelectMany(traverser =>
                        {
                            var bulk = traverser["bulk"]["@value"].ToObject<int>();
                            var value = traverser["value"]["@value"];

                            return AsyncEnumerable.Repeat(value, bulk);
                        })
                        .Select(value => new JArray(value))
                        .Catch<JToken, Exception>(ex =>
                        {
                            _logger?.LogError("Error executing Gremlin query {0}.", bytecode);

                            return AsyncEnumerableEx.Throw<JToken>(ex);
                        });
                }

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
