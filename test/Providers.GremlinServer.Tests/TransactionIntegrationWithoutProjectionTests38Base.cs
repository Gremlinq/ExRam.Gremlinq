using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/> answering like <see cref="TransactionIntegrationWithoutProjectionTests38"/>:
    /// Gremlin Server 3.8.2 with <c>TinkerTransactionGraph</c> without element projection, where every request runs in a transaction,
    /// as on Neptune. Every fact verifies against the snapshot <see cref="IntegrationWithoutProjectionTests38Base"/> names for it; one
    /// that lists the properties of a vertex compares canonically, since the graph returns them in another order. The facts on a failed
    /// write run here only, after every fact of <see cref="QueryExecutionTestsBase"/>, and verify against snapshots of
    /// <see cref="TransactionIntegrationWithoutProjectionTests38"/>, the leaf that owns them.
    /// </summary>
    public abstract class TransactionIntegrationWithoutProjectionTests38Base : IntegrationWithoutProjectionTests38Base
    {
        private const string RolledBackMarker = "rolled-back";

        protected TransactionIntegrationWithoutProjectionTests38Base(GremlinqFixture fixture, GremlinQueryVerifier verifier) : base(fixture, verifier)
        {
        }

        // TinkerTransactionGraph returns the properties of the vertex that has a FoundingDate and Locations in another order than
        // TinkerGraph also where they are typed. Values and ids are the same, so this fact compares canonically against the snapshot
        // of TinkerGraph, like the facts that IntegrationWithoutProjectionTestsBase compares canonically.
        public override SettingsTask Properties_typed_no_parameters() => base.Properties_typed_no_parameters().UseCanonicalComparer();

        // Gremlin defines no order for the meta properties of a vertex property either. TinkerTransactionGraph returns those
        // of the phone number with a MetaKey and a MetaProperty in another order than TinkerGraph.
        public override SettingsTask Properties_Meta_Values() => base.Properties_Meta_Values().UseCanonicalComparer();

        public override SettingsTask Properties_Values_typed() => base.Properties_Values_typed().UseCanonicalComparer();

        public override SettingsTask Properties_Values_untyped() => base.Properties_Values_untyped().UseCanonicalComparer();

        // The Write facts sort after every fact of QueryExecutionTestsBase, so that the ids the failed query uses up move no other
        // fact's ids and no other fact reads their vertices. Nothing sorts between them.
        [Fact]
        public virtual SettingsTask Write_of_a_failed_query_1_fails() => _g
            .AddV(new SimpleVertex { IetfLanguageTag = RolledBackMarker })
            .Fail("The vertex of this query is rolled back.")
            .Verify()
            .UseSnapshotDirectoryAndNameOf<TransactionIntegrationWithoutProjectionTests38>();

        // The vertex of the failed query is gone, but its id stays used up: the id of the next vertex is at least one higher, here
        // two, since vertices and vertex properties share one counter. The count is taken before the next vertex is added.
        [Fact]
        public virtual SettingsTask Write_of_a_failed_query_2_is_rolled_back() => _g
            .V<SimpleVertex>()
            .Where(x => x.IetfLanguageTag == RolledBackMarker)
            .Count()
            .Project(_ => _
                .ToDynamic()
                .By("verticesOfTheFailedQuery", __ => __.Identity())
                .By("idOfTheNextVertex", __ => __
                    .AddV(new SimpleVertex())
                    .Id()))
            .Verify()
            .UseSnapshotDirectoryAndNameOf<TransactionIntegrationWithoutProjectionTests38>();
    }
}
