using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Core.Tests.Verifiers;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Providers.JanusGraph.Tests
{
    public sealed class SerializationTests : QueryExecutionTest, IClassFixture<JanusGraphFixture>
    {
        public SerializationTests(JanusGraphFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new SerializingVerifier<Bytecode>(),
            testOutputHelper)
        {

        }
    }
}
