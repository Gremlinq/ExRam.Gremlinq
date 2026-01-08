using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public class Graphson3BinaryMessageSerializationTest : QueryExecutionTest, IClassFixture<NeptuneFixture>
    {
        public Graphson3BinaryMessageSerializationTest(NeptuneFixture fixture) : base(
            fixture,
            new BinaryMessageSerializingVerifier<GraphSon3BinaryMessage>())
        {
        }
    }
}
