using System;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using Microsoft.Azure.Cosmos;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public sealed class CosmosDbEmulatorFixture : IIntegrationTestFixture
    {
        private readonly Task _task;

        public CosmosDbEmulatorFixture()
        {
            _task = CreateImpl();

            GremlinQuerySource = g
                .UseCosmosDb(builder => builder
                    .At(new Uri("ws://localhost:8901"), "db", "graph")
                    .AuthenticateBy(
                        "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="))
                .ConfigureEnvironment(env => env
                    .AddFakePartitionKey());
        }

        public Task Create()
        {
            return _task;
        }

        private static async Task CreateImpl()
        {
            var cosmosClient = new CosmosClient("https://localhost:8081", "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");

            var database = await cosmosClient.CreateDatabaseIfNotExistsAsync("db", ThroughputProperties.CreateAutoscaleThroughput(40000));
            await database.Database.CreateContainerIfNotExistsAsync("graph", "/PartitionKey");
        }

        public IGremlinQuerySource GremlinQuerySource { get; }
    }
}
