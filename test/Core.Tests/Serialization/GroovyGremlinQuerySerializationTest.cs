using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class GroovyGremlinQuerySerializationTest : QueryExecutionTest, IClassFixture<GroovyGremlinQuerySerializationFixture>
    {
        public GroovyGremlinQuerySerializationTest(GroovyGremlinQuerySerializationFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new SerializingVerifier<GroovyGremlinQuery>(),
            testOutputHelper)
        {
        }
    }
}
