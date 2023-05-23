﻿using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Providers.CosmosDb.Tests.Extensions;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json.Linq;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public sealed class IntegrationCosmosDbFixture : GremlinqFixture
    {
        private readonly Task _task;

        public IntegrationCosmosDbFixture() : base(g
            .UseCosmosDb(builder => builder
                .At(new Uri("ws://localhost:8901"), "db", "graph")
                .AuthenticateBy(
                    "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==")
                .UseNewtonsoftJson())
            .ConfigureEnvironment(env => env
                .AddFakePartitionKey()
                .ConfigureDeserializer(d => d
                    .Add(ConverterFactory
                        .Create<JToken, JToken>((token, env, recurse) => token)))))
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
}
