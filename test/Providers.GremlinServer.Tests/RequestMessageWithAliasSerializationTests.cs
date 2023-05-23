using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Core.Tests.Verifiers;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class RequestMessageWithAliasSerializationTests : QueryExecutionTest, IClassFixture<RequestMessageWithAliasGremlinServerFixture>
    {
        public RequestMessageWithAliasSerializationTests(RequestMessageWithAliasGremlinServerFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new SerializingVerifier<RequestMessage>(),
            testOutputHelper)
        {

        }
    }
}
