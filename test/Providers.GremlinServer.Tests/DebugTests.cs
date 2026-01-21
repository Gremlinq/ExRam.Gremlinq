using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public class DebugTests : QueryExecutionTest, IClassFixture<GremlinServerContainerFixture>
    {
        public DebugTests(GremlinServerContainerFixture fixture) : base(
            fixture,
            new DebugGremlinQueryVerifier())
        {
        }
    }
}
