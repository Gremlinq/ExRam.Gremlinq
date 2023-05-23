using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Core.Tests.Verifiers;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public sealed class RequestMessageSerializationTests : QueryExecutionTest, IClassFixture<CosmosDbFixture>
    {
        public RequestMessageSerializationTests(CosmosDbFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new SerializingVerifier<RequestMessage>(),
            testOutputHelper)
        {

        }
    }
}
