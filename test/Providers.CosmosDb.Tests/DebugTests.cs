using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public sealed class DebugTests : QueryExecutionTest, IClassFixture<DebugTests.Fixture>
    {
        public sealed class Fixture : GremlinqTestFixture
        {
            public Fixture() : base(g
                .UseCosmosDb(_ => _
                    .At("ws://localhost", "", "")
                    .AuthenticateBy("")))
            {
            }
        }

        public DebugTests(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
