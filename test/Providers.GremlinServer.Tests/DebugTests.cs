using ExRam.Gremlinq.Core.Tests;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class DebugTests : QueryExecutionTest, IClassFixture<GremlinServerFixture>
    {
        public DebugTests(GremlinServerFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new DebugGremlinQueryVerifier(),
            testOutputHelper)
        {
        }
    }
}
