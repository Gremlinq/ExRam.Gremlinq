using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.TestCases;
using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class Graphson2GremlinQuerySerializationTest : QueryExecutionTest
    {
        public static readonly GraphSON2Writer GraphSon2Writer = new ();

        public Graphson2GremlinQuerySerializationTest(ITestOutputHelper testOutputHelper) : base(
            GremlinqFixture.Empty,
            new GraphSonStringSerializingVerifier(GraphSon2Writer),
            testOutputHelper)
        {
        }
    }
}
