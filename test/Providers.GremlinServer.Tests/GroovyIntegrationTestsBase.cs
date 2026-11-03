using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/> answering like <see cref="GroovyIntegrationTests"/>: every query sent as a Groovy
    /// script, against the customized Gremlin Server 3.7.7. A fact that the script engine answers differently verifies against the
    /// snapshot of <see cref="GroovyIntegrationTests"/>, the leaf that owns these answers; every other fact answers like
    /// <see cref="IntegrationTests"/>.
    /// </summary>
    public abstract class GroovyIntegrationTestsBase : IntegrationTestsBase
    {
        protected GroovyIntegrationTestsBase(GremlinqFixture fixture, GremlinQueryVerifier verifier) : base(fixture, verifier)
        {
        }

        // Sent as a script, these queries answer in another order or multiplicity where Gremlin defines none, or fail
        // with the error of the script engine instead of the error of the bytecode.
        public override SettingsTask Choose_Predicate1() => base.Choose_Predicate1().UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests>();

        public override SettingsTask Choose_Predicate3() => base.Choose_Predicate3().UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests>();

        public override SettingsTask Choose_Predicate4() => base.Choose_Predicate4().UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests>();

        public override SettingsTask Choose_Predicate5() => base.Choose_Predicate5().UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests>();

        public override SettingsTask Choose_Predicate8() => base.Choose_Predicate8().UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests>();

        public override SettingsTask Choose_Traversal2() => base.Choose_Traversal2().UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests>();

        public override SettingsTask Choose_unproductive() => base.Choose_unproductive().UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests>();

        public override SettingsTask Coalesce_with_2_not_matching_subQueries() => base.Coalesce_with_2_not_matching_subQueries().UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests>();

        public override SettingsTask E_baseType_Properties() => base.E_baseType_Properties().UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests>();

        public override SettingsTask E_Properties() => base.E_Properties().UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests>();

        public override SettingsTask Emit_Repeat() => base.Emit_Repeat().UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests>();

        public override SettingsTask Emit_Repeat_Times() => base.Emit_Repeat_Times().UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests>();

        public override SettingsTask Label() => base.Label().UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests>();

        public override SettingsTask Multi_Inject_V() => base.Multi_Inject_V().UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests>();

        public override SettingsTask Properties2() => base.Properties2().UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests>();

        public override SettingsTask Properties_Meta_ValueMap() => base.Properties_Meta_ValueMap().UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests>();

        public override SettingsTask Properties_ValueMap_typed() => base.Properties_ValueMap_typed().UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests>();

        public override SettingsTask Properties_ValueMap_untyped() => base.Properties_ValueMap_untyped().UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests>();

        public override SettingsTask Properties_Values_Id_Label() => base.Properties_Values_Id_Label().UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests>();

        public override SettingsTask Properties_Values_Label() => base.Properties_Values_Label().UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests>();

        public override SettingsTask StringKey() => base.StringKey().UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests>();

        public override SettingsTask V_BothE() => base.V_BothE().UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests>();

        public override SettingsTask V_BothE_3() => base.V_BothE_3().UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests>();

        public override SettingsTask Value() => base.Value().UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests>();

        public override SettingsTask Values_2_members() => base.Values_2_members().UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests>();

        public override SettingsTask Values_3_members() => base.Values_3_members().UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests>();

        public override SettingsTask Values_no_member() => base.Values_no_member().UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests>();
        public override SettingsTask Where_Id_equals_toStringed_Guid() => base.Where_Id_equals_toStringed_Guid().UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests>();
    }
}
