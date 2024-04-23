using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.TestCases;
using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class Graphson3GremlinQuerySerializationTest : QueryExecutionTest, IClassFixture<EmptyGremlinqTestFixture>
    {
        private static readonly GraphSON3Writer GraphSon3Writer = new ();

        public Graphson3GremlinQuerySerializationTest(EmptyGremlinqTestFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new GraphSonStringSerializingVerifier(GraphSon3Writer),
            testOutputHelper)
        {
        }
    }
}
