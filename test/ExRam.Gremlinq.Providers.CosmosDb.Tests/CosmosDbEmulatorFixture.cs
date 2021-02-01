using System.Threading.Tasks;

using Microsoft.Azure.Cosmos;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public sealed class CosmosDbEmulatorFixture
    {
        private readonly Task _task;

        public CosmosDbEmulatorFixture()
        {
            _task = CreateImpl();
        }

        public Task Create()
        {
            return _task;
        }

        private async Task CreateImpl()
        {
            var cosmosClient = new CosmosClient("https://localhost:8081", "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");

            var database = await cosmosClient.CreateDatabaseIfNotExistsAsync("db", ThroughputProperties.CreateAutoscaleThroughput(40000));
            await database.Database.CreateContainerIfNotExistsAsync("graph", "/PartitionKey");
        }
    }
}
