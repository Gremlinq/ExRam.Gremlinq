using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/> answering like <see cref="IntegrationTests"/>: Gremlin Server 3.7.7 through
    /// Newtonsoft.Json. A class that derives from here runs them in another environment and verifies against the snapshots of
    /// <see cref="IntegrationTests"/>, which is the leaf that owns them.
    /// </summary>
    public abstract class IntegrationTestsBase : QueryExecutionTestsBase
    {
        protected IntegrationTestsBase(GremlinqFixture fixture, GremlinQueryVerifier verifier) : base(fixture, verifier.WithSnapshotsOf<IntegrationTests>())
        {
        }
    }
}
