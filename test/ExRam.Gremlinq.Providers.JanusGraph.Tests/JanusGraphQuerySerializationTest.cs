using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.WebSocket;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.JanusGraph.Tests
{
    public sealed class JanusGraphQuerySerializationTest : QuerySerializationTest, IClassFixture<JanusGraphQuerySerializationTest.Fixture>
    {
        public sealed class Fixture : GremlinqTestFixture
        {
            public Fixture() : base(g
                .UseJanusGraph(builder => builder
                    .AtLocalhost()
                    .UseNewtonsoftJson())
                .ConfigureEnvironment(_ => _
                    .UseExecutor(GremlinQueryExecutor.Identity)))
            {
            }
        }

        public JanusGraphQuerySerializationTest(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {

        }
    }
}
