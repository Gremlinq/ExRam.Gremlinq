using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.Core;
using Gremlin.Net.Process.Traversal;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.JanusGraph.Tests
{
    public sealed class SerializationTests : SerializationTestsBase<Bytecode>, IClassFixture<SerializationTests.SerializationFixture>
    {
        public sealed class SerializationFixture : GremlinqTestFixture
        {
            public SerializationFixture() : base(g
                .UseJanusGraph(builder => builder
                    .AtLocalhost()))
            {
            }
        }

        public SerializationTests(SerializationFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {

        }
    }
}
