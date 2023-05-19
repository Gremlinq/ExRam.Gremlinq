using Gremlin.Net.Process.Traversal;
using static ExRam.Gremlinq.Core.GremlinQuerySource;
using static ExRam.Gremlinq.Core.Tests.BytecodeQuerySerializationTest;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class BytecodeQuerySerializationTest : SerializationTestsBase, IClassFixture<BytecodeQueryFixture>
    {
        public sealed class BytecodeQueryFixture : SerializationTestsFixture<Bytecode>
        {
            public BytecodeQueryFixture() : base(g.ConfigureEnvironment(_ => _))
            {
            }
        }

        public BytecodeQuerySerializationTest(BytecodeQueryFixture fixture, ITestOutputHelper testOutputHelper) : base(fixture, testOutputHelper)
        {
        }
    }
}
