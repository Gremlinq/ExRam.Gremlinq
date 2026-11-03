using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/> answering like <see cref="IntegrationWithoutProjectionTests"/>: Gremlin Server
    /// 3.7.7 without element projection, so that an element comes back as the server returns it. A class that derives from here
    /// runs them in another environment and verifies against the snapshots of <see cref="IntegrationWithoutProjectionTests"/>, which
    /// is the leaf that owns them.
    /// </summary>
    public abstract class IntegrationWithoutProjectionTestsBase : QueryExecutionTestsBase
    {
        protected IntegrationWithoutProjectionTestsBase(GremlinqFixture fixture, GremlinQueryVerifier verifier) : base(fixture, verifier.WithSnapshotsOf<IntegrationWithoutProjectionTests>())
        {
        }
    }
}
