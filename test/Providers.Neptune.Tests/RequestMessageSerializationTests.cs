using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public class RequestMessageSerializationTests : QueryExecutionTest, IClassFixture<NeptuneFixture>
    {
        public RequestMessageSerializationTests(NeptuneFixture fixture) : base(
            fixture,
            new SerializingVerifier<RequestMessage>())
        {
        }
    }
}
