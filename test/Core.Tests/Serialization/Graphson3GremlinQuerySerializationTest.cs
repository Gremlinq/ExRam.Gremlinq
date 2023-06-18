using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class Graphson3GremlinQuerySerializationTest : QueryExecutionTest, IClassFixture<GraphSon3StringSerializationFixture>
    {
        public Graphson3GremlinQuerySerializationTest(GraphSon3StringSerializationFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new SerializingVerifier<string>(),
            testOutputHelper)
        {
        }
    }
}
