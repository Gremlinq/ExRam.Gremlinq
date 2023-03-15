using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class GremlinServerDictionaryDeserializationTests : QueryDeserializationTest, IClassFixture<GremlinServerDictionaryDeserializationTests.Fixture>
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

        public GremlinServerDictionaryDeserializationTests(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }

        public override Task Verify<TElement>(IGremlinQueryBase<TElement> query) => base.Verify(query.Cast<object>());
    }
}
