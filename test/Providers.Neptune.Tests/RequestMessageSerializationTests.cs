using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Core.Tests.Verifiers;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public sealed class RequestMessageSerializationTests : QueryExecutionTest, IClassFixture<NeptuneFixture>
    {
        public RequestMessageSerializationTests(NeptuneFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new SerializingVerifier<RequestMessage>(),
            testOutputHelper)
        {
        }
    }
}
