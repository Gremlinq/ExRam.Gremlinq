using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Core.Tests.Fixtures;
using ExRam.Gremlinq.Providers.CosmosDb.Tests.Extensions;
using Gremlin.Net.Driver.Messages;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public sealed class RequestMessageSerializationTests : QueryExecutionTest, IClassFixture<RequestMessageSerializationTests.RequestMessageFixture>
    {
        public sealed class RequestMessageFixture : SerializationFixture<RequestMessage>
        {
            public RequestMessageFixture() : base(g
                .UseCosmosDb(builder => builder
                    .At(new Uri("wss://localhost"), "database", "graph")
                    .AuthenticateBy("authKey"))
                .ConfigureEnvironment(env => env
                    .AddFakePartitionKey()))
            {
            }
        }

        public RequestMessageSerializationTests(RequestMessageFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            GremlinQueryVerifier.Default,
            testOutputHelper)
        {

        }
    }
}
