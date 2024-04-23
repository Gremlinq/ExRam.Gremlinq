using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.TestCases;
using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class Graphson2GremlinQuerySerializationTest : QueryExecutionTest, IClassFixture<EmptyGremlinqTestFixture>
    {
        private static readonly GraphSON2Writer GraphSon2Writer = new ();

        public Graphson2GremlinQuerySerializationTest(EmptyGremlinqTestFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new GraphSonStringSerializingVerifier(GraphSon2Writer),
            testOutputHelper)
        {
        }
    }
}
