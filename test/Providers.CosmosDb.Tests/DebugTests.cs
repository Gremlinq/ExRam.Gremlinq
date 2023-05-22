using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public sealed class DebugTests : QueryExecutionTest, IClassFixture<DebugTests.DebugFixture>
    {
        public sealed class DebugFixture : GremlinqTestFixture
        {
            public DebugFixture() : base(g
                .UseCosmosDb(_ => _
                    .At("ws://localhost", "", "")
                    .AuthenticateBy("")))
            {
            }
        }

        public DebugTests(DebugFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            GremlinQueryVerifier.Default,
            testOutputHelper)
        {
        }
    }
}
