using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public class DeserializationTests : QueryExecutionTest, IClassFixture<GremlinServerFixture>
    {
        public DeserializationTests(GremlinServerFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new DeserializingGremlinqVerifier<IntegrationTests>(testOutputHelper))
        {
        }
       
    }
}
