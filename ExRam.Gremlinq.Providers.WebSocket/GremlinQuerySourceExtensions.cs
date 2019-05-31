using System;
using System.Collections.Generic;
using System.Linq;
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
                private readonly IGremlinClient _gremlinClient;
                private readonly IGraphsonDeserializerFactory _graphSonDeserializerFactory;

                public WebSocketGremlinQueryExecutor(
                    IGremlinClient client,
                    IGraphsonDeserializerFactory graphSonDeserializerFactory,
                    ILogger logger = null)
                {
                    _logger = logger;
                    _gremlinClient = client;
                    _graphSonDeserializerFactory = graphSonDeserializerFactory;
                }

                public void Dispose()
                {
                    _gremlinClient.Dispose();
                }

                public IAsyncEnumerable<TElement> Execute<TElement>(IGremlinQuery<TElement> query)
                {
                    var visitor = query.AsAdmin().Visitors.Get<SerializedGremlinQuery>();

                    visitor
                        .Visit(query);

                    var serialized = visitor.Build();

                    _logger?.LogTrace("Executing Gremlin query {0}.", serialized.QueryString);

                    return _gremlinClient
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
            private readonly IGremlinClient _client;
            private readonly IGraphsonDeserializerFactory _deserializer;

            public DefaultWebSocketRemoteConfigurator(
                [AllowNull] IGremlinClient client,
                [AllowNull] IGraphsonDeserializerFactory deserializer,
                ILogger logger)
            {
                _client = client;
                _logger = logger;
                _deserializer = deserializer;
            }

            public IWebSocketRemoteConfigurator WithClient(IGremlinClient client)
            {
                return new DefaultWebSocketRemoteConfigurator(client, _deserializer, _logger);
            }

            public IWebSocketRemoteConfigurator WithSerializerFactory(IGraphsonDeserializerFactory deserializer)
            {
                return new DefaultWebSocketRemoteConfigurator(_client, deserializer, _logger);
            }

            public IGremlinQueryExecutor Build()
            {
                if (_client == null || _deserializer == null)
                    throw new InvalidOperationException($"{nameof(WithClient)} and {nameof(WithSerializerFactory)} must be called on the {nameof(IWebSocketRemoteConfigurator)}.");

                return new WebSocketGremlinQueryExecutor(_client, _deserializer, _logger);
            }
        }

        public static IConfigurableGremlinQuerySource WithRemote(this IConfigurableGremlinQuerySource source, string hostname, GraphsonVersion graphsonVersion, int port = 8182, bool enableSsl = false, string username = null, string password = null)
        {
            return source.WithRemote(
                new GremlinServer(hostname, port, enableSsl, username, password),
                graphsonVersion);
        }

        public static IConfigurableGremlinQuerySource WithRemote(this IConfigurableGremlinQuerySource source, GremlinServer server, GraphsonVersion graphsonVersion)
        {
            return source.ConfigureWebSocketRemote(conf => conf
                .WithClient(new GremlinClient(
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
        
        public static IConfigurableGremlinQuerySource ConfigureWebSocketRemote(this IConfigurableGremlinQuerySource source, Func<IWebSocketRemoteConfigurator, IWebSocketRemoteConfigurator> transformation)
        {
            return source
                .ConfigureVisitors(_ => _.TryAdd<SerializedGremlinQuery, GroovyGremlinQueryElementVisitor>())
                .WithExecutor(transformation(new DefaultWebSocketRemoteConfigurator(null, null, source.Logger)).Build());
        }
    }
}
