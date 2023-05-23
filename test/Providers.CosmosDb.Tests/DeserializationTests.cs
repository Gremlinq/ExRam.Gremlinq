using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.CosmosDb.Tests.Fixtures;
using ExRam.Gremlinq.Support.NewtonsoftJson.Tests;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public sealed class DeserializationTests : QueryExecutionTest, IClassFixture<SimpleCosmosDbFixture>
    {
        public DeserializationTests(SimpleCosmosDbFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new DeserializingGremlinqVerifier(testOutputHelper),
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
