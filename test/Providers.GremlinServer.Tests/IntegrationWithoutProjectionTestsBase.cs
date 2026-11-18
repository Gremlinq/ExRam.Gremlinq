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

        // Gremlin defines no order for the properties of an element. The server returns those of the vertex that has a FoundingDate
        // and Locations in an order that is not the same everywhere: Locations before or after FoundingDate. Values and ids are the
        // same, so these facts compare canonically.
        public override SettingsTask Group_with_key_and_value2() => base.Group_with_key_and_value2().UseCanonicalComparer();

        public override SettingsTask Properties1() => base.Properties1().UseCanonicalComparer();

        public override SettingsTask Properties_Values_Id() => base.Properties_Values_Id().UseCanonicalComparer();

        public override SettingsTask Properties_Values_Id_Label() => base.Properties_Values_Id_Label().UseCanonicalComparer();

        public override SettingsTask Properties_Values_Label() => base.Properties_Values_Label().UseCanonicalComparer();

        public override SettingsTask Value() => base.Value().UseCanonicalComparer();
    }
}
