namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class DefaultDebugGremlinQuerySerializationTest : QueryExecutionTest, IClassFixture<EmptyGremlinqTestFixture>
    {
        public DefaultDebugGremlinQuerySerializationTest(EmptyGremlinqTestFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
