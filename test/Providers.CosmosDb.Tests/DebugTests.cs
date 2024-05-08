using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public sealed class DebugTests : QueryExecutionTest, IClassFixture<CosmosDbFixture>
    {
        public DebugTests(CosmosDbFixture fixture) : base(
            fixture,
            new DebugGremlinQueryVerifier())
        {
        }
    }
}
