using Xunit;
using Xunit.Abstractions;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class DefaultDebugGremlinQuerySerializationTest : DebugGremlinQuerySerializationTest, IClassFixture<DebugGremlinQuerySerializationTest.Fixture>
    {
        public DefaultDebugGremlinQuerySerializationTest(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
