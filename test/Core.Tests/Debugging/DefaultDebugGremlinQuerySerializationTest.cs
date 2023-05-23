using ExRam.Gremlinq.Tests.Infrastructure;

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
