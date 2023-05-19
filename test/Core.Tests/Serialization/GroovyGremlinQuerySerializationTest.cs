using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Core.Tests.Fixtures;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class GroovyGremlinQuerySerializationTest : SerializationTestsBase, IClassFixture<GroovySerializationFixture>
    {
        public GroovyGremlinQuerySerializationTest(GroovySerializationFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
