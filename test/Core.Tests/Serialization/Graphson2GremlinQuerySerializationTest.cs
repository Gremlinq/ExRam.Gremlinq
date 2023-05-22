using ExRam.Gremlinq.Core.Tests.Fixtures;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class Graphson2GremlinQuerySerializationTest : QueryExecutionTest, IClassFixture<Graphson2StringSerializationFixture>
    {
        public Graphson2GremlinQuerySerializationTest(Graphson2StringSerializationFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new SerializingVerifier<string>(),
            testOutputHelper)
        {
        }
    }
}
