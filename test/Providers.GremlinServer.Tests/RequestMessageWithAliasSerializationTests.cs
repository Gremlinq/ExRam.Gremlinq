using ExRam.Gremlinq.Tests.Infrastructure;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class RequestMessageWithAliasSerializationTests : QueryExecutionTestsBase, IClassFixture<RequestMessageWithAliasGremlinServerFixture>
    {
        public RequestMessageWithAliasSerializationTests(RequestMessageWithAliasGremlinServerFixture fixture) : base(
            fixture,
            new SerializingVerifier<RequestMessage>())
        {

        }
    }
}
