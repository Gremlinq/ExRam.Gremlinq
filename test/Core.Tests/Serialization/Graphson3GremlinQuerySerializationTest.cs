using ExRam.Gremlinq.Core.Tests.Fixtures;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class Graphson3GremlinQuerySerializationTest : SerializationTestsBase, IClassFixture<GraphSon3StringFixture>
    {
        public Graphson3GremlinQuerySerializationTest(GraphSon3StringFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
