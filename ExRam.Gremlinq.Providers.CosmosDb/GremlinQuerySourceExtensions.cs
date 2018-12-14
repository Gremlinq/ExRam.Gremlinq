using ExRam.Gremlinq.Providers.CosmosDb;
using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq
{
    public static class GremlinQuerySourceExtensions
    {
        public static IGremlinQuerySource WithCosmosDbRemote(this IGremlinQuerySource source, string hostname, string database, string graphName, string authKey, int port = 443, bool enableSsl = true)
        {
            return source.WithRemote(
                new CosmosDbGremlinServer(hostname, database, graphName, authKey, port, enableSsl),
                GraphsonVersion.v2);
        }
    }
}
