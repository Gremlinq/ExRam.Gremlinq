using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Core.Tests
{
    public class DefaultDebugGremlinQuerySerializationTests : QueryExecutionTestsBase, IClassFixture<GremlinqFixture>
    {
        public DefaultDebugGremlinQuerySerializationTests(GremlinqFixture fixture) : base(
            fixture,
            new DebugVerifier())
        {
        }
    }
}
