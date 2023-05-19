using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.Core;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class DebugTests : QueryExecutionTest, IClassFixture<DebugTests.DebugFixture>
    {
        public sealed class DebugFixture : GremlinqTestFixture
        {
            public DebugFixture() : base(g
                .UseGremlinServer(_ => _
                    .AtLocalhost()))
            {
            }
        }

        public DebugTests(DebugFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
