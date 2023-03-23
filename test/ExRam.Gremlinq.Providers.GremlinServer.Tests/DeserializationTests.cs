using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Support.NewtonsoftJson.Tests;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class DeserializationTests : DeserializationTestsBase, IClassFixture<DeserializationTests.Fixture>
    {
        public new sealed class Fixture : DeserializationTestsBase.Fixture
        {
            public Fixture() : base(
                nameof(IntegrationTests),
                g
                    .UseGremlinServer(_ => _
                        .UseNewtonsoftJson()))
            {
            }
        }

        public DeserializationTests(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
