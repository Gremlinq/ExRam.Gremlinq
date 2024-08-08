using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    [IntegrationTest("Windows", true)]
    public class IntegrationTests : QueryExecutionTest, IClassFixture<CosmosDbEmulatorFixture>
    {
        public IntegrationTests(CosmosDbEmulatorFixture fixture) : base(
            fixture,
            new JTokenExecutingVerifier())
        {
        }
    }
}
