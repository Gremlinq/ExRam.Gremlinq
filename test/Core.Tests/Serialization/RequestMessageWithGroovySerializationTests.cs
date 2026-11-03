using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class RequestMessageWithGroovySerializationTests : QueryExecutionTestsBase, IClassFixture<GroovyGremlinQuerySerializationFixture>
    {
        public RequestMessageWithGroovySerializationTests(GroovyGremlinQuerySerializationFixture fixture) : base(
            fixture,
            new SerializingVerifier<RequestMessage>())
        {
        }
    }
}
