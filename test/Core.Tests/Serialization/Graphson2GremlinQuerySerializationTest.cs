using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;

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
