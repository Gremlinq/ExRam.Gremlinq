using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.GremlinServer.Tests.Fixtures;
using ExRam.Gremlinq.Support.NewtonsoftJson.Tests;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class DeserializationTests : QueryExecutionTest, IClassFixture<SimpleGremlinServerFixture>
    {
        public DeserializationTests(SimpleGremlinServerFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new DeserializingGremlinqVerifier(testOutputHelper),
            testOutputHelper)
        {
        }
    }
}
