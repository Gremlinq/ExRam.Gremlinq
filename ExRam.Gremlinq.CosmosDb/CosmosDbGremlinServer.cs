using Gremlin.Net.Driver;

namespace ExRam.Gremlinq.CosmosDb
{
    public class CosmosDbGremlinServer : GremlinServer
    {
        public CosmosDbGremlinServer(string hostname, int port = 8182, bool enableSsl = false, string database = null, string graphName = null, string authKey = null) : base(
            hostname,
            port,
            enableSsl,
            $"/dbs/{database}/colls/{graphName}",
            authKey)
        {
        }
    }
}