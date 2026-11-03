using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/> serialized to bytecode by the Neptune provider for an engine that understands
    /// TinkerPop 3.7 only, answering like <see cref="SerializationTests"/>, the leaf that owns the snapshots.
    /// </summary>
    public abstract class SerializationTestsBase : QueryExecutionTestsBase
    {
        protected SerializationTestsBase(GremlinqFixture fixture, GremlinQueryVerifier verifier) : base(fixture, verifier.WithSnapshotsOf<SerializationTests>())
        {
        }
    }
}
