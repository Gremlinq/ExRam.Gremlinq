using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Support.NewtonsoftJson.Tests;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class DeserializationTests : QueryExecutionTest, IClassFixture<GremlinServerFixture>
    {
        public DeserializationTests(GremlinServerFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new DeserializingGremlinqVerifier(testOutputHelper),
            testOutputHelper)
        {
        }
    }
}
