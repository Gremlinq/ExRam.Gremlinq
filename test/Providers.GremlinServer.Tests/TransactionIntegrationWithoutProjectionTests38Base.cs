using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/> answering like <see cref="TransactionIntegrationWithoutProjectionTests38"/>:
    /// Gremlin Server 3.8.2 with <c>TinkerTransactionGraph</c> without element projection, where every request runs in a transaction,
    /// as on Neptune. Every fact verifies against the snapshot <see cref="IntegrationWithoutProjectionTests38Base"/> names for it; one
    /// that lists the properties of a vertex compares canonically, since the graph returns them in another order.
    /// </summary>
    public abstract class TransactionIntegrationWithoutProjectionTests38Base : IntegrationWithoutProjectionTests38Base
    {
        protected TransactionIntegrationWithoutProjectionTests38Base(GremlinqFixture fixture, GremlinQueryVerifier verifier) : base(fixture, verifier)
        {
        }

        // Gremlin defines no order for the properties of an element. TinkerTransactionGraph returns those of the vertex that has a
        // FoundingDate and Locations in another order than TinkerGraph: Locations before FoundingDate. Values and ids are the same,
        // so these facts compare canonically against the snapshots of TinkerGraph.
        public override SettingsTask Group_with_key_and_value2() => base.Group_with_key_and_value2().UseCanonicalComparer();

        public override SettingsTask Properties1() => base.Properties1().UseCanonicalComparer();

        public override SettingsTask Properties_typed_no_parameters() => base.Properties_typed_no_parameters().UseCanonicalComparer();

        public override SettingsTask Properties_Values_Id() => base.Properties_Values_Id().UseCanonicalComparer();

        public override SettingsTask Properties_Values_Id_Label() => base.Properties_Values_Id_Label().UseCanonicalComparer();

        public override SettingsTask Properties_Values_Label() => base.Properties_Values_Label().UseCanonicalComparer();

        public override SettingsTask Value() => base.Value().UseCanonicalComparer();
    }
}
