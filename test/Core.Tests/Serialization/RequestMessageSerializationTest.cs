using ExRam.Gremlinq.Tests.Infrastructure;

using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Core.Tests
{
    public class RequestMessageSerializationTest : QueryExecutionTest, IClassFixture<GremlinqFixture>
    {
        public RequestMessageSerializationTest(GremlinqFixture fixture) : base(
            fixture,
            new SerializingVerifier<RequestMessage>())
        {
        }
    }
}
