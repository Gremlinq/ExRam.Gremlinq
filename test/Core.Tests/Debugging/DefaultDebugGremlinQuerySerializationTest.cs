using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Core.Tests
{
    public class DefaultDebugGremlinQuerySerializationTest : QueryExecutionTest, IClassFixture<GremlinqFixture>
    {
        public DefaultDebugGremlinQuerySerializationTest(GremlinqFixture fixture) : base(
            fixture,
            new DebugGremlinQueryVerifier())
        {
        }
    }
}
