using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Serialization;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    public static class GremlinQuerySourceExtensions
    {
        private sealed class GremlinClientEx : GremlinClient
        {
            // ReSharper disable once InconsistentNaming
            private sealed class NullGraphSSON2Reader : GraphSON2Reader
            {
                public override dynamic ToObject(JToken jToken)
                {
                    return new[] { jToken };
                }
            }

            // ReSharper disable once InconsistentNaming
            private sealed class NullGraphSSON3Reader : GraphSON3Reader
            {
                public override dynamic ToObject(JToken jToken)
                {
                    return new[] { jToken };
                }
            }

            public GremlinClientEx(GremlinServer gremlinServer, GraphsonVersion version) : base(
                gremlinServer,
                version == GraphsonVersion.V2
                    ? new NullGraphSSON2Reader()
                    : (GraphSONReader)new NullGraphSSON3Reader(),
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
