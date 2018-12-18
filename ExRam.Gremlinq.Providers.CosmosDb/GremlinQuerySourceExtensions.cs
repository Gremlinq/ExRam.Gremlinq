using ExRam.Gremlinq.Providers.CosmosDb;
using ExRam.Gremlinq.Providers.WebSocket;
using ExRam.Gremlinq.Serialization;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq
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

        public static IGremlinQuerySource WithCosmosDbRemote(this IGremlinQuerySource source, string hostname, string database, string graphName, string authKey, int port = 443, bool enableSsl = true)
        {
            return source.WithExecutor(
                new WebSocketGremlinQueryExecutor(
                    new GremlinClientEx(
                        new CosmosDbGremlinServer(hostname, database, graphName, authKey, port, enableSsl),
                        GraphsonVersion.V2),
                    new StringGremlinQuerySerializer<CosmosDbGroovyGremlinQueryElementVisitor>()));
        }
    }
}
