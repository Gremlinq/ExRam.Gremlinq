using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.Core;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class GremlinServerQuerySerializationTest : QuerySerializationTest<BytecodeGremlinQuery>, IClassFixture<GremlinServerQuerySerializationTest.Fixture>
    {
        public sealed class Fixture : GremlinqTestFixture
        {
            public Fixture() : base(g
                .UseGremlinServer(builder => builder
                    .AtLocalhost()
                    .UseNewtonsoftJson()))
            {
            }
        }

        public GremlinServerQuerySerializationTest(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {

        }
    }
}
