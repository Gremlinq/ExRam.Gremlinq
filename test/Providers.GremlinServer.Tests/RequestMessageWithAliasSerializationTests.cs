using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class RequestMessageWithAliasSerializationTests : QueryExecutionTest, IClassFixture<RequestMessageWithAliasGremlinServerFixture>
    {
        public RequestMessageWithAliasSerializationTests(RequestMessageWithAliasGremlinServerFixture fixture) : base(
            fixture,
            new SerializingVerifier<RequestMessage>())
        {

        }
    }
}
