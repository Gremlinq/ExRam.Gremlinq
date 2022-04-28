using ExRam.Gremlinq.Core.Serialization;
using Xunit;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class InlinedGroovyGremlinQuerySerializationTest : QuerySerializationTest, IClassFixture<InlinedGroovyGremlinQuerySerializationTest.Fixture>
    {
        public new sealed class Fixture : QuerySerializationTest.Fixture
        {
            public Fixture() : base(g.ConfigureEnvironment(_ => _
                .ConfigureSerializer(_ => _.ToGroovy(GroovyFormatting.Inline))))
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
