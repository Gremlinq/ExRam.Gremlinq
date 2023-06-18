using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class BytecodeQuerySerializationTest : QueryExecutionTest
    {
        public BytecodeQuerySerializationTest(ITestOutputHelper testOutputHelper) : base(GremlinqFixture.Empty, new SerializingVerifier<Bytecode>(), testOutputHelper)
        {
        }
    }
}
