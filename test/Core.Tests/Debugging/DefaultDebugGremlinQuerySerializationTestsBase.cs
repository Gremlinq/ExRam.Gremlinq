using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Core.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/> as the default debugger prints them, answering like
    /// <see cref="DefaultDebugGremlinQuerySerializationTests"/>, the leaf that owns the snapshots.
    /// </summary>
    public abstract class DefaultDebugGremlinQuerySerializationTestsBase : QueryExecutionTestsBase
    {
        protected DefaultDebugGremlinQuerySerializationTestsBase(GremlinqFixture fixture, GremlinQueryVerifier verifier) : base(fixture, verifier.WithSnapshotsOf<DefaultDebugGremlinQuerySerializationTests>())
        {
        }
    }
}
