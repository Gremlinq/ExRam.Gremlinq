using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.CosmosDb;
using ExRam.Gremlinq.Providers.WebSocket;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class CosmosDbDebugGremlinQuerySerializationTest : DebugGremlinQuerySerializationTest, IClassFixture<CosmosDbDebugGremlinQuerySerializationTest.Fixture>
    {
        public sealed class Fixture : GremlinqTestFixture
        {
            public Fixture() : base(g
                .UseCosmosDb(_ => _
                    .At("ws://localhost", "", "")
                    .AuthenticateBy(""))
                .UseDebuggingExecutor())
            {
            }
        }

        public CosmosDbDebugGremlinQuerySerializationTest(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
