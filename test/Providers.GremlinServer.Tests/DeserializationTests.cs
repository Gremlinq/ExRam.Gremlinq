using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class DeserializationTests : QueryExecutionTest, IClassFixture<GremlinServerFixture>
    {
        public DeserializationTests(GremlinServerFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new DeserializingGremlinqVerifier<IntegrationTests>(testOutputHelper),
            testOutputHelper)
        {
        }
    }
}
