using ExRam.Gremlinq.Providers.CosmosDb;
using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq
{
    public static class GremlinQuerySourceExtensions
    {
        public static IGremlinQuerySource WithCosmosDbRemote(this IGremlinQuerySource source, string hostname, string database, string graphName, string authKey, int port = 8182, bool enableSsl = false)
        {
            return source
                .WithQueryProvider(new ClientGremlinQueryProvider(new CosmosDbGremlinServer(hostname, database, graphName, authKey, port, enableSsl)));
        }
    }
}
