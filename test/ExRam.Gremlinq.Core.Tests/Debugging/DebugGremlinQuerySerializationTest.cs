using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class DebugGremlinQuerySerializationTest : QueryExecutionTest
    {
        public sealed class Fixture : GremlinqTestFixture
        {
            public Fixture() : base(g
                .UseDebuggingExecutor())
            {
            }
        }

        public DebugGremlinQuerySerializationTest(GremlinqTestFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }

        public override Task Verify<TElement>(IGremlinQueryBase<TElement> query) => base.Verify(query.Cast<string>());
    }
}
