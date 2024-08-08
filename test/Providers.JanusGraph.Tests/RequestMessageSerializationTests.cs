using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.JanusGraph.Tests
{
    public class RequestMessageSerializationTests : QueryExecutionTest, IClassFixture<JanusGraphFixture>
    {
        public RequestMessageSerializationTests(JanusGraphFixture fixture) : base(
            fixture,
            new SerializingVerifier<RequestMessage>())
        {

        }
    }
}
