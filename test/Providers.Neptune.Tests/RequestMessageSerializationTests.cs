using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Core.Tests.Verifiers;
using ExRam.Gremlinq.Providers.Neptune.Tests.Fixtures;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public sealed class RequestMessageSerializationTests : QueryExecutionTest, IClassFixture<SimpleNeptuneFixture>
    {
        public RequestMessageSerializationTests(SimpleNeptuneFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new SerializingVerifier<RequestMessage>(),
            testOutputHelper)
        {
        }
    }
}
