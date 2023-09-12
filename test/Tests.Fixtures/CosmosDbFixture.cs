using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.CosmosDb;
using ExRam.Gremlinq.Providers.CosmosDb.Tests.Extensions;
using Microsoft.Azure.Cosmos;
using Polly;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public sealed class CosmosDbFixture : GremlinqFixture
    {
        private const string CosmosDbEmulatorDatabaseName = "db";
        private static readonly string CosmosDbEmulatorCollectionName = "graph";
        private const string CosmosDbEmulatorAuthKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        private readonly Task _task;

        public CosmosDbFixture() : base()
        {
            _task = Task.Run(
                async () =>
                {
                    var cosmosClient = new CosmosClient("https://localhost:8081", CosmosDbEmulatorAuthKey);

                    await Policy
                        .Handle<CosmosException>()
                        .WaitAndRetry(8, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                        .Execute(async () =>
                        {
                            var database = await cosmosClient.CreateDatabaseIfNotExistsAsync(CosmosDbEmulatorDatabaseName, ThroughputProperties.CreateAutoscaleThroughput(40000));
                            await database.Database.CreateContainerIfNotExistsAsync(CosmosDbEmulatorCollectionName, "/PartitionKey");
                        });
                });
        }

        protected override async Task<IGremlinQuerySource> TransformQuerySource(IConfigurableGremlinQuerySource g) => g
            .UseCosmosDb(builder => builder
                .At(new Uri("ws://localhost:8901"), CosmosDbEmulatorDatabaseName, CosmosDbEmulatorCollectionName)
                .AuthenticateBy(CosmosDbEmulatorAuthKey)
                .UseNewtonsoftJson())
            .ConfigureEnvironment(env => env
                .AddFakePartitionKey());

        public Task Create() => _task;
    }
}
