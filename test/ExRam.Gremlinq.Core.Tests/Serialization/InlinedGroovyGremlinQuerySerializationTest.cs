using Xunit;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class InlinedGroovyGremlinQuerySerializationTest : QuerySerializationTest, IClassFixture<InlinedGroovyGremlinQuerySerializationTest.Fixture>
    {
        public sealed class Fixture : GremlinqTestFixture
        {
            public Fixture() : base(g.ConfigureEnvironment(_ => _
                .UseSerializer(GremlinQuerySerializer.Default.ToGroovy(GroovyFormatting.Inline))))
            {
            }
        }

        public InlinedGroovyGremlinQuerySerializationTest(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
