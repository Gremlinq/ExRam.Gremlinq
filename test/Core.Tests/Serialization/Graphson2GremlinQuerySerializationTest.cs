using ExRam.Gremlinq.Core.Tests.Fixtures;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class Graphson2GremlinQuerySerializationTest : SerializationTestsBase, IClassFixture<Graphson2StringFixture>
    {
        public Graphson2GremlinQuerySerializationTest(Graphson2StringFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
