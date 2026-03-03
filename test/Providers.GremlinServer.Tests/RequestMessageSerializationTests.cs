using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public class RequestMessageSerializationTests : QueryExecutionTest, IClassFixture<GremlinServerContainerFixture>
    {
        public RequestMessageSerializationTests(GremlinServerContainerFixture fixture) : base(
            fixture,
            new SerializingVerifier<RequestMessage>())
        {

        }
    }
}
