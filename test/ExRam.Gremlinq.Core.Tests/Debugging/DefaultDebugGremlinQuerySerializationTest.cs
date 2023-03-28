using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class DefaultDebugGremlinQuerySerializationTest : QueryExecutionTest, IClassFixture<DefaultDebugGremlinQuerySerializationTest.Fixture>
    {
        public sealed class Fixture : GremlinqTestFixture
        {
            public Fixture() : base(g.ConfigureEnvironment(_ => _))
            {
            }
        }

        public DefaultDebugGremlinQuerySerializationTest(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
