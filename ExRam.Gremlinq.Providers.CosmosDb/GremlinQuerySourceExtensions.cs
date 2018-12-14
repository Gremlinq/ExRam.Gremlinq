using ExRam.Gremlinq.Providers.CosmosDb;
using ExRam.Gremlinq.Providers.WebSocket;

namespace ExRam.Gremlinq
{
    public static class GremlinQuerySourceExtensions
    {
        public static IGremlinQuerySource WithCosmosDbRemote(this IGremlinQuerySource source, string hostname, int port = 8182, bool enableSsl = false, string database = null, string graphName = null, string authKey = null)
        {
            return source
                .WithQueryProvider(new ClientGremlinQueryProvider(new CosmosDbGremlinServer(hostname, port, enableSsl, database, graphName, authKey)));
        }
    }
}
