using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.WebSocket;
using Xunit;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class GremlinServerQuerySerializationTest : QuerySerializationTest, IClassFixture<GremlinServerQuerySerializationTest.Fixture>
    {
        public new sealed class Fixture : QuerySerializationTest.Fixture
        {
            public Fixture() : base(g
                .UseGremlinServer(builder => builder
                    .AtLocalhost()))
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
