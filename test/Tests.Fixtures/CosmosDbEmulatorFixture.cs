using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.CosmosDb;
using ExRam.Gremlinq.Providers.CosmosDb.Tests.Extensions;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Support.NewtonsoftJson;

using Microsoft.Azure.Cosmos;
using Polly;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public class CosmosDbEmulatorFixture : GremlinqFixture
    {
        private const string CosmosDbEmulatorCollectionName = "graphs";
        private const string CosmosDbEmulatorAuthKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        protected override async Task<IGremlinQuerySource> TransformQuerySource(IGremlinQuerySource g)
        {
            var databaseName = Guid.NewGuid().ToString("N");

            using (var cosmosClient = new CosmosClient("https://localhost:8081", CosmosDbEmulatorAuthKey))
            {
                await Policy
                    .Handle<CosmosException>()
                    .WaitAndRetry(8, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                    .Execute(async () =>
                    {
                        var database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName, ThroughputProperties.CreateAutoscaleThroughput(40000));

                        await database.Database.CreateContainerIfNotExistsAsync(CosmosDbEmulatorCollectionName, "/PartitionKey");
                    });

                return g
                    .UseCosmosDb<Vertex, Edge>(conf => conf
                        .At(new Uri("ws://localhost:8901"), databaseName, CosmosDbEmulatorCollectionName)
                        .AuthenticateBy(CosmosDbEmulatorAuthKey)
                        .WithPartitionKey(x => x.Label!)
                        .UseNewtonsoftJson())
                    .ConfigureEnvironment(env => env
                        .AddFakePartitionKey()
                        .ConfigureOptions(options => options
                            .SetValue(GremlinqOption.StringComparisonTranslationStrictness, StringComparisonTranslationStrictness.Lenient)));
            }
        }
    }
}
