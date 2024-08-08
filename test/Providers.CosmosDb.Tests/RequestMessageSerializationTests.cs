using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public class RequestMessageSerializationTests : QueryExecutionTest, IClassFixture<CosmosDbFixture>
    {
        public RequestMessageSerializationTests(CosmosDbFixture fixture) : base(
            fixture,
            new SerializingVerifier<RequestMessage>())
        {

        }
    }
}
