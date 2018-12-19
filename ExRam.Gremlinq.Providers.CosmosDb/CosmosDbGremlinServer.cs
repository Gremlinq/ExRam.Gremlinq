using Gremlin.Net.Driver;

namespace ExRam.Gremlinq.Providers.CosmosDb
{
    public class CosmosDbGremlinServer : GremlinServer
    {
        public CosmosDbGremlinServer(string hostname, string database, string graphName, string authKey, int port = 443) : base(
            hostname,
            port,
            true,
            $"/dbs/{database}/colls/{graphName}",
            authKey)
        {
        }
    }
}
