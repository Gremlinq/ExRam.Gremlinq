using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public abstract class CosmosDbIntegrationTestBase : CosmosDbTestBase
    {
        protected CosmosDbIntegrationTestBase(GremlinqFixture fixture, GremlinQueryVerifier verifier) : base(fixture, verifier)
        {
        }

        [Fact(Skip = "id as key cannot be scrubbed.")]
        public override Task Group_with_key_identity() => base.Group_with_key_identity();
    }
}
