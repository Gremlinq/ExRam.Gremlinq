using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;

using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class Graphson3WithGroovyGremlinQuerySerializationTest : QueryExecutionTest, IClassFixture<GroovyGremlinQuerySerializationFixture>
    {
        public Graphson3WithGroovyGremlinQuerySerializationTest(GroovyGremlinQuerySerializationFixture fixture) : base(
            fixture,
            new SerializingVerifier<RequestMessage>())
        {
        }
    }
}
