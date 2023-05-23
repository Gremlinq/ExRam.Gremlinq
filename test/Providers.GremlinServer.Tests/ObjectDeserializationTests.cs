using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Support.NewtonsoftJson.Tests;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class ObjectDeserializationTests : QueryExecutionTest, IClassFixture<GremlinServerFixture>
    {
        public ObjectDeserializationTests(GremlinServerFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new ObjectDeserializingGremlinqVerifier(testOutputHelper),
            testOutputHelper)
        {
        }
    }
}
