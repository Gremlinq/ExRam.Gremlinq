using System;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Serialization;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using Microsoft.Extensions.Logging;
using NullGuard;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public static class GremlinQuerySourceExtensions
    {
        private sealed class DefaultWebSocketRemoteConfigurator : IWebSocketRemoteConfigurator
        {
            private readonly ILogger _logger;
            private readonly IGremlinClient _client;
            private readonly IGraphsonSerializerFactory _serializer;

            public DefaultWebSocketRemoteConfigurator(
                [AllowNull] IGremlinClient client,
                [AllowNull] IGraphsonSerializerFactory serializer,
                ILogger logger)
            {
                _client = client;
                _logger = logger;
                _serializer = serializer;
            }

            public IWebSocketRemoteConfigurator WithClient(IGremlinClient client)
            {
                return new DefaultWebSocketRemoteConfigurator(client, _serializer, _logger);
            }

            public IWebSocketRemoteConfigurator WithSerializerFactory(IGraphsonSerializerFactory serializer)
            {
                return new DefaultWebSocketRemoteConfigurator(_client, serializer, _logger);
            }

            public IGremlinQueryExecutor Build()
            {
                if (_client == null || _serializer == null)
                    throw new InvalidOperationException($"{nameof(WithClient)} and {nameof(WithSerializerFactory)} must be called on the {nameof(IWebSocketRemoteConfigurator)}.");

                return new WebSocketGremlinQueryExecutor(_client, _serializer, _logger);
            }
        }

        private sealed class GremlinClientEx : GremlinClient
        {
            public GremlinClientEx(GremlinServer gremlinServer, GraphsonVersion version) : base(
                gremlinServer,
                version == GraphsonVersion.V2
                    ? new GraphSON2Reader()
                    : (GraphSONReader)new GraphSON3Reader(),
                version == GraphsonVersion.V2
                    ? new GraphSON2Writer()
                    : (GraphSONWriter)new GraphSON3Writer(),
                version == GraphsonVersion.V2
                    ? GraphSON2MimeType
                    : DefaultMimeType)
            {

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
                .WithClient(new GremlinClientEx(
                    server,
                    graphsonVersion))
                .WithSerializerFactory(new DefaultGraphsonSerializerFactory()));
        }
        
        public static IConfigurableGremlinQuerySource ConfigureWebSocketRemote(this IConfigurableGremlinQuerySource source, Func<IWebSocketRemoteConfigurator, IWebSocketRemoteConfigurator> transformation)
        {
            return source
                .WithExecutor(transformation(new DefaultWebSocketRemoteConfigurator(null, null, source.Logger)).Build());
        }
    }
}
