using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Core.Tests.Verifiers;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.JanusGraph.Tests
{
    public sealed class RequestMessageSerializationTests : QueryExecutionTest, IClassFixture<JanusGraphFixture>
    {
        public RequestMessageSerializationTests(JanusGraphFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new SerializingVerifier<RequestMessage>(),
            testOutputHelper)
        {

        }
    }
}
