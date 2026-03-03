using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public class SerializationTests : QueryExecutionTest, IClassFixture<GremlinServerContainerFixture>
    {
        public SerializationTests(GremlinServerContainerFixture fixture) : base(
            fixture,
            new SerializingVerifier<Bytecode>())
        {

        }
    }
}
