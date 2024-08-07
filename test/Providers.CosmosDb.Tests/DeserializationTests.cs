using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public class DeserializationTests : QueryExecutionTest, IClassFixture<CosmosDbFixture>
    {
        public DeserializationTests(CosmosDbFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new DeserializingGremlinqVerifier<IntegrationTests>(testOutputHelper))
        {
        }

        [Fact(Skip="Wrong Choose signature in Gremlinq <= 8!")]
        public override Task Choose_Predicate2()
        {
            return base.Choose_Predicate2();
        }

        [Fact(Skip = "CosmosDb maps from id to vertices instead of identity vertex to vertices.")]
        public override Task Group()
        {
            return base.Group();
        }
    }
}
