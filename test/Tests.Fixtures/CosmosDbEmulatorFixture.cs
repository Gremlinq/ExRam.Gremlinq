using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Providers.CosmosDb;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using System.Text.Json;
using Microsoft.Azure.Cosmos;
using Polly;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public class CosmosDbEmulatorFixture : GremlinqFixture
    {
        private const string CosmosDbEmulatorDatabaseName = "db";
        private const string CosmosDbEmulatorAuthKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        private readonly string _collectionName = Guid.NewGuid().ToString("N");
        private bool _containerCreated;

        protected override async Task<IGremlinQuerySource> TransformQuerySource(IGremlinQuerySource g)
        {
            using (var cosmosClient = new CosmosClient("https://localhost:8081", CosmosDbEmulatorAuthKey))
            {
                await Policy
                    .Handle<CosmosException>()
                    .WaitAndRetry(8, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                    .Execute(async () =>
                    {
                        var database = await cosmosClient.CreateDatabaseIfNotExistsAsync(CosmosDbEmulatorDatabaseName, ThroughputProperties.CreateAutoscaleThroughput(40000));

                        var containerResponse = await database.Database.CreateContainerIfNotExistsAsync(_collectionName, "/PartitionKey");
                    });

                _containerCreated = true;

                return g
                    .UseCosmosDb<Vertex, Edge>(conf => conf
                        .At(new Uri("ws://localhost:8901"), CosmosDbEmulatorDatabaseName, _collectionName)
                        .AuthenticateBy(CosmosDbEmulatorAuthKey)
                        .WithPartitionKey(x => x.PartitionKey!)
                        .UseNewtonsoftJson()
                        .ConfigureClientFactory(factory => factory
                            .ConfigureClient(client => client
                                .ObserveResultStatusAttributes((_, attributes) =>
                                {
                                    Console.WriteLine(JsonSerializer.Serialize(attributes));
                                }))))
                    .ConfigureEnvironment(env => env
                        .ConfigureOptions(options => options
                            .SetValue(GremlinqOption.StringComparisonTranslationStrictness, StringComparisonTranslationStrictness.Lenient)));
            }
        }

        public override async Task DisposeAsync()
        {
            if (_containerCreated)
            {
                using (var cosmosClient = new CosmosClient("https://localhost:8081", CosmosDbEmulatorAuthKey))
                {
                    await cosmosClient
                        .GetDatabase(CosmosDbEmulatorDatabaseName)
                        .GetContainer(_collectionName)
                        .DeleteContainerAsync();
                }
            }

            await base.DisposeAsync();
        }
    }
}
