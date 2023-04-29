using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class BytecodeQuerySerializationTest : SerializationTestsBase<Bytecode>
    {
        public BytecodeQuerySerializationTest(ITestOutputHelper testOutputHelper) : base(
            GremlinqTestFixture.Empty,
            testOutputHelper)
        {
        }
    }
}
