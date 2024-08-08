using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Core.Tests
{
    public class GroovyGremlinQuerySerializationTest : QueryExecutionTest, IClassFixture<GroovyGremlinQuerySerializationFixture>
    {
        public GroovyGremlinQuerySerializationTest(GroovyGremlinQuerySerializationFixture fixture) : base(
            fixture,
            new SerializingVerifier<GroovyGremlinScript>())
        {
        }
    }
}
