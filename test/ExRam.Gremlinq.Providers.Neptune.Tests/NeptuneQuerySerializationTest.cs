using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.Core;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public sealed class NeptuneQuerySerializationTest : QuerySerializationTest, IClassFixture<NeptuneQuerySerializationTest.Fixture>
    {
        public sealed class Fixture : GremlinqTestFixture
        {
            public Fixture() : base(g
                .UseNeptune(builder => builder
                    .AtLocalhost()
                    .UseNewtonsoftJson())
                .ConfigureEnvironment(_ => _
                    .UseExecutor(GremlinQueryExecutor.Identity)))
            {
            }
        }

        public NeptuneQuerySerializationTest(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
