using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Serialization;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using NullGuard;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public static class GremlinQuerySourceExtensions
    {
        private sealed class DefaultWebSocketRemoteConfigurator : IWebSocketRemoteConfigurator
        {
            private class WebSocketGremlinQueryExecutor : IGremlinQueryExecutor, IDisposable
            {
                private readonly ILogger _logger;
                private readonly Lazy<IGremlinClient> _lazyGremlinClient;
                private readonly IGraphsonDeserializerFactory _graphSonDeserializerFactory;

                public WebSocketGremlinQueryExecutor(
                    Func<IGremlinClient> clientFactory,
                    IGraphsonDeserializerFactory graphSonDeserializerFactory,
                    ILogger logger = null)
                {
                    _logger = logger;
                    _graphSonDeserializerFactory = graphSonDeserializerFactory;
                    _lazyGremlinClient = new Lazy<IGremlinClient>(clientFactory, LazyThreadSafetyMode.ExecutionAndPublication);
                }

                public void Dispose()
                {
                    _lazyGremlinClient.Value.Dispose();
                }

                public IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query)
                {
                    var visitor = query
                        .AsAdmin().Visitors
                        .TryGet<SerializedGremlinQuery>()
                        .IfNone(() => throw new InvalidOperationException($"{nameof(query)} does not contain an {nameof(IGremlinQueryElementVisitor)} for {nameof(SerializedGremlinQuery)}."));

                    visitor
                        .Visit(query);

                    var serialized = visitor.Build();

                    _logger?.LogTrace("Executing Gremlin query {0}.", serialized.QueryString);

                    return _lazyGremlinClient
                        .Value
                        .SubmitAsync<JToken>(serialized.QueryString, serialized.Bindings)
                        .ToAsyncEnumerable()
                        .SelectMany(x => x
                            .ToAsyncEnumerable())
                        .Catch<JToken, Exception>(ex =>
                        {
                            _logger?.LogError("Error executing Gremlin query {0}.", serialized.QueryString);

                            return AsyncEnumerableEx.Throw<JToken>(ex);
                        })
                        .GraphsonDeserialize<TElement[]>(_graphSonDeserializerFactory.Get(query.AsAdmin().Model))
                        .SelectMany(x => x.ToAsyncEnumerable());
                }
            }

            private readonly ILogger _logger;
            private readonly Func<IGremlinClient> _clientFactory;
            private readonly IGraphsonDeserializerFactory _deserializer;

            public DefaultWebSocketRemoteConfigurator(
                [AllowNull] Func<IGremlinClient> clientFactory,
                [AllowNull] IGraphsonDeserializerFactory deserializer,
                ILogger logger)
            {
                _logger = logger;
                _deserializer = deserializer;
                _clientFactory = clientFactory;
            }

            public IWebSocketRemoteConfigurator WithClientFactory(Func<IGremlinClient> clientFactory)
            {
                return new DefaultWebSocketRemoteConfigurator(clientFactory, _deserializer, _logger);
            }

            public IWebSocketRemoteConfigurator WithSerializerFactory(IGraphsonDeserializerFactory deserializer)
            {
                return new DefaultWebSocketRemoteConfigurator(_clientFactory, deserializer, _logger);
            }

            public IGremlinQueryExecutor Build()
            {
                if (_clientFactory == null || _deserializer == null)
                    throw new InvalidOperationException($"{nameof(WithClientFactory)} and {nameof(WithSerializerFactory)} must be called on the {nameof(IWebSocketRemoteConfigurator)}.");

                return new WebSocketGremlinQueryExecutor(_clientFactory, _deserializer, _logger);
            }
        }

        public static IConfigurableGremlinQuerySource WithWebSocket(this IConfigurableGremlinQuerySource source, string hostname, GraphsonVersion graphsonVersion, int port = 8182, bool enableSsl = false, string username = null, string password = null)
        {
            return source.WithWebSocket(
                new GremlinServer(hostname, port, enableSsl, username, password),
                graphsonVersion);
        }

        public static IConfigurableGremlinQuerySource WithWebSocket(this IConfigurableGremlinQuerySource source, GremlinServer server, GraphsonVersion graphsonVersion)
        {
            return source.ConfigureWebSocket(conf => conf
                .WithClientFactory(() => new GremlinClient(
                    server,
                    graphsonVersion == GraphsonVersion.V2
                        ? new GraphSON2Reader()
                        : (GraphSONReader)new GraphSON3Reader(),
                    graphsonVersion == GraphsonVersion.V2
                        ? new GraphSON2Writer()
                        : (GraphSONWriter)new GraphSON3Writer(),
                    graphsonVersion == GraphsonVersion.V2
                        ? GremlinClient.GraphSON2MimeType
                        : GremlinClient.DefaultMimeType))
                .WithSerializerFactory(new DefaultGraphsonDeserializerFactory()));
        }
        
        public static IConfigurableGremlinQuerySource ConfigureWebSocket(this IConfigurableGremlinQuerySource source, Func<IWebSocketRemoteConfigurator, IWebSocketRemoteConfigurator> transformation)
        {
            return source
                .ConfigureVisitors(_ => _.TryAdd<SerializedGremlinQuery, GroovyGremlinQueryElementVisitor>())
                .WithExecutor(transformation(new DefaultWebSocketRemoteConfigurator(null, null, source.Logger)).Build());
        }
    }
}
