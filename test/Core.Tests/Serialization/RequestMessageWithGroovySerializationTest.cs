using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Core.Tests
{
    public class RequestMessageWithGroovySerializationTest : QueryExecutionTest, IClassFixture<GroovyGremlinQuerySerializationFixture>
    {
        public RequestMessageWithGroovySerializationTest(GroovyGremlinQuerySerializationFixture fixture) : base(
            fixture,
            new SerializingVerifier<RequestMessage>())
        {
        }
    }
}
