using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Core.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/> serialized to bytecode, answering like
    /// <see cref="BytecodeQuerySerializationTests"/>, the leaf that owns the snapshots. A provider's serialization test class
    /// derives from here and overrides the facts its provider serializes differently.
    /// </summary>
    public abstract class BytecodeQuerySerializationTestsBase : QueryExecutionTestsBase
    {
        protected BytecodeQuerySerializationTestsBase(GremlinqFixture fixture, GremlinQueryVerifier verifier) : base(fixture, verifier.WithSnapshotsOf<BytecodeQuerySerializationTests>())
        {
        }
    }
}
