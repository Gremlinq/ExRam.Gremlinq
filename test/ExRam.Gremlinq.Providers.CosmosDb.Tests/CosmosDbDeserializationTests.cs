using System.Threading.Tasks;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using Xunit;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public sealed class CosmosDbDeserializationTests : QueryDeserializationTest, IClassFixture<CosmosDbDeserializationTests.Fixture>
    {
        public sealed class Fixture : IntegrationTestFixture
        {
            public Fixture() : base(g
                .UseCosmosDb(_ => _.At("ws://localhost", "", "").AuthenticateBy("")))
            {
            }
        }

        public CosmosDbDeserializationTests(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
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
