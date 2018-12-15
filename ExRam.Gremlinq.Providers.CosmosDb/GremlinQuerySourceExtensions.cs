using ExRam.Gremlinq.Providers.CosmosDb;
using ExRam.Gremlinq.Providers.WebSocket;
using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public static class GremlinQuerySourceExtensions
    {
        public static IGremlinQuerySource WithCosmosDbRemote(this IGremlinQuerySource source, string hostname, string database, string graphName, string authKey, int port = 443, bool enableSsl = true)
        {
            return source.WithQueryProvider(
                new ClientGremlinQueryProvider(
                    new GremlinClientEx(
                        new CosmosDbGremlinServer(hostname, database, graphName, authKey, port, enableSsl),
                        GraphsonVersion.V2),
                    new StringGremlinQuerySerializer<CosmosDbGroovyGremlinQueryElementVisitor>()));
        }
    }
}
