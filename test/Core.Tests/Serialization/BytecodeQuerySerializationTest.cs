using ExRam.Gremlinq.Core.Tests.Fixtures;
using Gremlin.Net.Process.Traversal;
using static ExRam.Gremlinq.Core.GremlinQuerySource;
using static ExRam.Gremlinq.Core.Tests.BytecodeQuerySerializationTest;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class BytecodeQuerySerializationTest : QueryExecutionTest, IClassFixture<BytecodeQueryFixture>
    {
        public sealed class BytecodeQueryFixture : GremlinqTestFixture
        {
            public BytecodeQueryFixture() : base(g.ConfigureEnvironment(_ => _))
            {
            }
        }

        public BytecodeQuerySerializationTest(BytecodeQueryFixture fixture, ITestOutputHelper testOutputHelper) : base(fixture, new SerializingVerifier<Bytecode>(), testOutputHelper)
        {
        }
    }
}
