using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class DeserializationWithoutProjectionTests : QueryExecutionTest, IClassFixture<GremlinServerFixture>
    {
        public DeserializationWithoutProjectionTests(GremlinServerFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new DeserializingGremlinqVerifier<IntegrationWithoutProjectionTests>(testOutputHelper),
            testOutputHelper)
        {
        }
    }
}
