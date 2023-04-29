using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class BytecodeQuerySerializationTest : SerializationTestsBase<Bytecode>, IClassFixture<EmptyGremlinqTestFixture>
    {
        public BytecodeQuerySerializationTest(EmptyGremlinqTestFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
