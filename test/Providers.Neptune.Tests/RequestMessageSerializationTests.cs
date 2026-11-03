using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public sealed class RequestMessageSerializationTests : QueryExecutionTestsBase, IClassFixture<NeptuneContainerFixture>
    {
        public RequestMessageSerializationTests(NeptuneContainerFixture fixture) : base(
            fixture,
            new SerializingVerifier<RequestMessage>())
        {
        }
    }
}
