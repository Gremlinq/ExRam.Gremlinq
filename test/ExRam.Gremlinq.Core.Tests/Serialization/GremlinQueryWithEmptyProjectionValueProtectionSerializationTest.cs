using ExRam.Gremlinq.Core.Execution;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class GremlinQueryWithEmptyProjectionValueProtectionSerializationTest : QuerySerializationTest, IClassFixture<GremlinQueryWithEmptyProjectionValueProtectionSerializationTest.Fixture>
    {
        public sealed class Fixture : GremlinqTestFixture
        {
            public Fixture() : base(g
                .ConfigureEnvironment(_ => _
                    .ConfigureOptions(o => o.SetValue(GremlinqOption.EnableEmptyProjectionValueProtection, true))
                    .UseExecutor(GremlinQueryExecutor.Identity)))
            {
            }
        }

        public GremlinQueryWithEmptyProjectionValueProtectionSerializationTest(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
