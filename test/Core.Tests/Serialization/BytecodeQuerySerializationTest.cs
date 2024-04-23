using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class BytecodeQuerySerializationTest : QueryExecutionTest, IClassFixture<GremlinqFixture>
    {
        public BytecodeQuerySerializationTest(GremlinqFixture fixture, ITestOutputHelper testOutputHelper) : base(fixture, new SerializingVerifier<Bytecode>(), testOutputHelper)
        {
        }
    }
}
