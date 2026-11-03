using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Providers.JanusGraph.Tests
{
    public sealed class SerializationTests : QueryExecutionTestsBase, IClassFixture<JanusGraphContainerFixture>
    {
        public SerializationTests(JanusGraphContainerFixture fixture) : base(
            fixture,
            new SerializingVerifier<Bytecode>())
        {

        }
    }
}
