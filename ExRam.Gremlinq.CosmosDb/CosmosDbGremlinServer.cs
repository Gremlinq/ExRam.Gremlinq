using Gremlin.Net.Driver;

namespace ExRam.Gremlinq.CosmosDb
{
    public class CosmosDbGremlinServer : GremlinServer
    {
        public CosmosDbGremlinServer(CosmosDbGraphConfiguration configuration) : base(
            configuration.Hostname,
            configuration.Port,
            configuration.EnableSsl,
            $"/dbs/{configuration.Database}/colls/{configuration.GraphName}",
            configuration.AuthKey)
        {

        }
    }
}