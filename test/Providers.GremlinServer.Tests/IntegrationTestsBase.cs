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

        // These queries walk the edges of a vertex in an order that depends on the ids the graph drew. A class with facts of its
        // own, such as IntegrationTests, draws other ids than one without, so these facts compare canonically.
        public override SettingsTask Optional() => base.Optional().UseCanonicalComparer();

        public override SettingsTask Repeat_Emit() => base.Repeat_Emit().UseCanonicalComparer();

        public override SettingsTask Repeat_Emit_Times() => base.Repeat_Emit_Times().UseCanonicalComparer();
    }
}
