using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Core.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/> serialized to a request message, answering like
    /// <see cref="RequestMessageSerializationTests"/>, the leaf that owns the snapshots.
    /// </summary>
    public abstract class RequestMessageSerializationTestsBase : QueryExecutionTestsBase
    {
        protected RequestMessageSerializationTestsBase(GremlinqFixture fixture, GremlinQueryVerifier verifier) : base(fixture, verifier.WithSnapshotsOf<RequestMessageSerializationTests>())
        {
        }
    }
}
