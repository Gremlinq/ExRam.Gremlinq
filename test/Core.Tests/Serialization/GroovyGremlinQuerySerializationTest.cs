using ExRam.Gremlinq.Core.Serialization;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class GroovyGremlinQuerySerializationTest : SerializationTestsBase<GroovyGremlinQuery>, IClassFixture<GroovyGremlinQuerySerializationTest.GroovyFixture>
    {
        public sealed class GroovyFixture : GremlinqTestFixture
        {
            public GroovyFixture() : base(g.ConfigureEnvironment(_ => _
                .ConfigureSerializer(ser => ser
                    .PreferGroovySerialization())))
            {
            }
        }

        public GroovyGremlinQuerySerializationTest(GroovyFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
