using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public class ObjectDeserializationTests : QueryExecutionTest, IClassFixture<GremlinServerFixture>
    {
        public ObjectDeserializationTests(GremlinServerFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new ObjectDeserializingGremlinqVerifier<IntegrationTests>(testOutputHelper))
        {
        }
    }
}
