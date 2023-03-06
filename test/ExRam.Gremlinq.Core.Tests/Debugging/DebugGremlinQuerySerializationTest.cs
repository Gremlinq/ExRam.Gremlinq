using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class DebugGremlinQuerySerializationTest : QueryExecutionTest
    {
        protected DebugGremlinQuerySerializationTest(GremlinqTestFixture fixture, ITestOutputHelper testOutputHelper, [CallerFilePath] string callerFilePath = "") : base(
            fixture,
            testOutputHelper,
            callerFilePath)
        {
        }

        public override Task Verify<TElement>(IGremlinQueryBase<TElement> query) => base.Verify(query.Cast<string>());

        protected override IImmutableList<Func<string, string>> Scrubbers() => base
            .Scrubbers()
            .ScrubGuids();
    }
}
