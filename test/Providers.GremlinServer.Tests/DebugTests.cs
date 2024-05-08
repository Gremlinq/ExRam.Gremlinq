using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class DebugTests : QueryExecutionTest, IClassFixture<GremlinServerFixture>
    {
        public DebugTests(GremlinServerFixture fixture) : base(
            fixture,
            new DebugGremlinQueryVerifier())
        {
        }
    }
}
