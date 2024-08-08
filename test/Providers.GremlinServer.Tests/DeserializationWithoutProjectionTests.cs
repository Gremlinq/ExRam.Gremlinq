using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public class DeserializationWithoutProjectionTests : QueryExecutionTest, IClassFixture<GremlinServerFixture>
    {
        public DeserializationWithoutProjectionTests(GremlinServerFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new DeserializingGremlinqVerifier<IntegrationWithoutProjectionTests>(testOutputHelper))
        {
        }
    }
}
