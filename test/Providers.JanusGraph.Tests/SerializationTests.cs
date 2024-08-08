using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Providers.JanusGraph.Tests
{
    public class SerializationTests : QueryExecutionTest, IClassFixture<JanusGraphFixture>
    {
        public SerializationTests(JanusGraphFixture fixture) : base(
            fixture,
            new SerializingVerifier<Bytecode>())
        {

        }
    }
}
