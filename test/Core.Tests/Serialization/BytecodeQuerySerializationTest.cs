using ExRam.Gremlinq.Tests.Infrastructure;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Tests
{
    public class BytecodeQuerySerializationTest : QueryExecutionTest, IClassFixture<GremlinqFixture>
    {
        public BytecodeQuerySerializationTest(GremlinqFixture fixture) : base(fixture, new SerializingVerifier<Bytecode>())
        {
        }
    }
}
