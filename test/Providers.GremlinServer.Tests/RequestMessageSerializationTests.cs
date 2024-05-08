using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class RequestMessageSerializationTests : QueryExecutionTest, IClassFixture<GremlinServerFixture>
    {
        public RequestMessageSerializationTests(GremlinServerFixture fixture) : base(
            fixture,
            new SerializingVerifier<RequestMessage>())
        {

        }
    }
}
