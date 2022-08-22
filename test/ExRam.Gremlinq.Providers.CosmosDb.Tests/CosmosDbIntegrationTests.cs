using System.Collections.Immutable;
using System.Text.RegularExpressions;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using Microsoft.Azure.Cosmos;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public class CosmosDbIntegrationTests : QueryIntegrationTest, IClassFixture<CosmosDbIntegrationTests.Fixture>
    {
        public new sealed class Fixture : QueryIntegrationTest.Fixture
        {
            private readonly Task _task;

            public Fixture() : base(Gremlinq.Core.GremlinQuerySource.g
                .UseCosmosDb(builder => builder
                    .At(new Uri("ws://localhost:8901"), "db", "graph")
                    .AuthenticateBy(
                        "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="))
                .ConfigureEnvironment(env => env
                    .AddFakePartitionKey()))
            {
                _task = CreateImpl();
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
        }

        private static readonly Regex IdRegex2 = new("\"[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}([|]PartitionKey)?\"", RegexOptions.IgnoreCase);

        public CosmosDbIntegrationTests(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
            fixture.Create().Wait();
        }

        protected override IImmutableList<Func<string, string>> Scrubbers()
        {
            return base.Scrubbers()
                .Add(x => IdRegex2.Replace(x, "\"scrubbed id\""));
        }
    }
}
