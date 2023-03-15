using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class GremlinServerDeserializationTests : QueryDeserializationTest, IClassFixture<GremlinServerDeserializationTests.Fixture>
    {
        public new sealed class Fixture : QueryDeserializationTest.Fixture
        {
            public Fixture() : base(
                nameof(GremlinServerIntegrationTests),
                g
                    .UseGremlinServer(_ => _
                        .UseNewtonsoftJson()))
            {
            }
        }

        public GremlinServerDeserializationTests(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
