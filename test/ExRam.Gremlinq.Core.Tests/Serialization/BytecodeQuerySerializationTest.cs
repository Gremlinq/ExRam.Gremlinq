using ExRam.Gremlinq.Core.Serialization;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class BytecodeQuerySerializationTest : SerializationTestsBase<BytecodeGremlinQuery>, IClassFixture<BytecodeQuerySerializationTest.Fixture>
    {
        public sealed class Fixture : GremlinqTestFixture
        {
            public Fixture() : base(g.ConfigureEnvironment(_ => _))
            {
            }
        }

        public BytecodeQuerySerializationTest(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
