namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class DefaultDebugGremlinQuerySerializationTest : QueryExecutionTest
    {
        public DefaultDebugGremlinQuerySerializationTest(ITestOutputHelper testOutputHelper) : base(
            GremlinqTestFixture.Empty,
            testOutputHelper)
        {
        }
    }
}
