using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Core.Tests
{
    public class Graphson2BinaryMessageSerializationTest : QueryExecutionTest, IClassFixture<GremlinqFixture>
    {
        public Graphson2BinaryMessageSerializationTest(GremlinqFixture fixture) : base(
            fixture,
            new BinaryMessageSerializingVerifier<GraphSon2BinaryMessage>())
        {
        }
    }
}
