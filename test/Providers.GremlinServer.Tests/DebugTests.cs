using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.GremlinServer.Tests.Fixtures;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class DebugTests : QueryExecutionTest, IClassFixture<SimpleGremlinServerFixture>
    {
        public DebugTests(SimpleGremlinServerFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new DebugGremlinQueryVerifier(),
            testOutputHelper)
        {
        }
    }
}
