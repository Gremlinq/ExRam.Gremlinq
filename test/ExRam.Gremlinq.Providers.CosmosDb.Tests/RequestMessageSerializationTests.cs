using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.CosmosDb.Tests.Extensions;
using Gremlin.Net.Driver.Messages;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public sealed class RequestMessageSerializationTests : SerializationTestsBase<RequestMessage>, IClassFixture<RequestMessageSerializationTests.Fixture>
    {
        public sealed class Fixture : GremlinqTestFixture
        {
            public Fixture() : base(g
                .UseCosmosDb(builder => builder
                    .At(new Uri("wss://localhost"), "database", "graph")
                    .AuthenticateBy("authKey"))
                .ConfigureEnvironment(env => env
                    .AddFakePartitionKey()))
            {
            }
        }

        public RequestMessageSerializationTests(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {

        }
    }
}
