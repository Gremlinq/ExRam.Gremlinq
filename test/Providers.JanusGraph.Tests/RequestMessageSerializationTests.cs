using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.JanusGraph.Tests
{
    public sealed class RequestMessageSerializationTests : QueryExecutionTestsBase, IClassFixture<JanusGraphContainerFixture>
    {
        public RequestMessageSerializationTests(JanusGraphContainerFixture fixture) : base(
            fixture,
            new SerializingVerifier<RequestMessage>())
        {

        }
    }
}
