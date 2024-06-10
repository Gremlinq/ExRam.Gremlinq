using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.TestCases;
using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class Graphson2BinaryMessageSerializationTest : QueryExecutionTest, IClassFixture<GremlinqFixture>
    {
        private static readonly GraphSON2Writer GraphSon2Writer = new ();

        public Graphson2BinaryMessageSerializationTest(GremlinqFixture fixture) : base(
            fixture,
            new GraphSonStringSerializingVerifier(GraphSon2Writer))
        {
        }
    }
}
