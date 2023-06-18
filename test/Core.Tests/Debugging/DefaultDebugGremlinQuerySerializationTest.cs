using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class DefaultDebugGremlinQuerySerializationTest : QueryExecutionTest
    {
        public DefaultDebugGremlinQuerySerializationTest(ITestOutputHelper testOutputHelper) : base(
            GremlinqFixture.Empty,
            new DebugGremlinQueryVerifier(),
            testOutputHelper)
        {
        }
    }
}
