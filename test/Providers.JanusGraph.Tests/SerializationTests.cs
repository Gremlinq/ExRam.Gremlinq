using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Providers.JanusGraph.Tests
{
    public sealed class SerializationTests : QueryExecutionTest, IClassFixture<JanusGraphFixture>
    {
        public SerializationTests(JanusGraphFixture fixture) : base(
            fixture,
            new SerializingVerifier<Bytecode>())
        {

        }
    }
}
