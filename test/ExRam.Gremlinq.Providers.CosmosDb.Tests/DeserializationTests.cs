using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Support.NewtonsoftJson.Tests;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public sealed class DeserializationTests : QueryDeserializationTest, IClassFixture<DeserializationTests.Fixture>
    {
        public new sealed class Fixture : QueryDeserializationTest.Fixture
        {
            public Fixture() : base(
                nameof(IntegrationTests),
                g.UseCosmosDb(_ => _
                    .At("ws://localhost", "", "")
                    .AuthenticateBy("")
                    .UseNewtonsoftJson()))
            {
            }
        }

        public DeserializationTests(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }

        [Fact(Skip="Wrong Choose signature in Gremlinq <= 8!")]
        public override Task Choose_Predicate2()
        {
            return base.Choose_Predicate2();
        }
    }
}
