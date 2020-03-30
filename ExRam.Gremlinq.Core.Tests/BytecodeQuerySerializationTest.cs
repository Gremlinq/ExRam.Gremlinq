using Xunit.Abstractions;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class BytecodeQuerySerializationTest : QuerySerializationTest
    {
        public BytecodeQuerySerializationTest(ITestOutputHelper testOutputHelper) : base(
            GremlinQuerySource.g.ConfigureEnvironment(_ => _),
            testOutputHelper)
        {
        }
    }
}
