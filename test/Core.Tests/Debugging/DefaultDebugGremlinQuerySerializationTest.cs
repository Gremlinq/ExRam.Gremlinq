using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class DefaultDebugGremlinQuerySerializationTest : QueryExecutionTest, IClassFixture<GremlinqFixture>
    {
        public DefaultDebugGremlinQuerySerializationTest(GremlinqFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new DebugGremlinQueryVerifier(),
            testOutputHelper)
        {
        }
    }
}
