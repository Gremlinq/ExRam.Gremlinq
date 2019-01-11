using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Serialization;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public static class GremlinQuerySourceExtensions
    {
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
            return source.WithExecutor(
                new WebSocketGremlinQueryExecutor<GroovyGremlinQueryElementVisitor>(
                    new GremlinClientEx(
                        server,
                        graphsonVersion),
                    new DefaultGraphsonSerializerFactory()));
        }
    }
}
