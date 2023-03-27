using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class DebugTestsBase : QueryExecutionTest
    {
        protected DebugTestsBase(GremlinqTestFixture fixture, ITestOutputHelper testOutputHelper, [CallerFilePath] string callerFilePath = "") : base(
            fixture,
            testOutputHelper,
            callerFilePath)
        {
        }

        public override Task Verify<TElement>(IGremlinQueryBase<TElement> query) => base.Verify(query.Debug());
    }
}
