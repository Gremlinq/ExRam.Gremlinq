using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public sealed class Graphson3BinaryMessageSerializationTests : QueryExecutionTestsBase, IClassFixture<NeptuneContainerFixture>
    {
        public Graphson3BinaryMessageSerializationTests(NeptuneContainerFixture fixture) : base(
            fixture,
            new BinaryMessageSerializingVerifier<GraphSon3BinaryMessage>())
        {
        }
    }
}
