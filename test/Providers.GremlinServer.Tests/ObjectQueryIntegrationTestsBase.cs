using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/> answering like <see cref="ObjectQueryIntegrationTests"/>: every query cast to
    /// <see cref="object"/>, against Gremlin Server 3.7.7. A fact whose answer keeps a form of the server's that the typed query
    /// converts verifies against the snapshot of <see cref="ObjectQueryIntegrationTests"/>, the leaf that owns these answers;
    /// every other fact answers like <see cref="IntegrationTests"/>.
    /// </summary>
    public abstract class ObjectQueryIntegrationTestsBase : IntegrationTestsBase
    {
        protected ObjectQueryIntegrationTestsBase(GremlinqFixture fixture, GremlinQueryVerifier verifier) : base(fixture, verifier)
        {
        }

        // Cast to object, a result keeps the form the server sent: a map keeps its T.id and T.label keys and a projection,
        // a tree or a property map stays a map instead of becoming the fact's type, and a mean stays a double.
        public override SettingsTask ElementMap() => base.ElementMap().UseSnapshotDirectoryAndNameOf<ObjectQueryIntegrationTests>();

        public override SettingsTask MeanGlobal() => base.MeanGlobal().UseSnapshotDirectoryAndNameOf<ObjectQueryIntegrationTests>();

        public override SettingsTask MeanLocal() => base.MeanLocal().UseSnapshotDirectoryAndNameOf<ObjectQueryIntegrationTests>();

        public override SettingsTask Path() => base.Path().UseSnapshotDirectoryAndNameOf<ObjectQueryIntegrationTests>();

        public override SettingsTask Project_for_coverage8() => base.Project_for_coverage8().UseSnapshotDirectoryAndNameOf<ObjectQueryIntegrationTests>();

        public override SettingsTask Project_for_coverage9() => base.Project_for_coverage9().UseSnapshotDirectoryAndNameOf<ObjectQueryIntegrationTests>();

        public override SettingsTask Project_for_coverage10() => base.Project_for_coverage10().UseSnapshotDirectoryAndNameOf<ObjectQueryIntegrationTests>();

        public override SettingsTask Project_for_coverage11() => base.Project_for_coverage11().UseSnapshotDirectoryAndNameOf<ObjectQueryIntegrationTests>();

        public override SettingsTask Project_for_coverage12() => base.Project_for_coverage12().UseSnapshotDirectoryAndNameOf<ObjectQueryIntegrationTests>();

        public override SettingsTask Project_for_coverage13() => base.Project_for_coverage13().UseSnapshotDirectoryAndNameOf<ObjectQueryIntegrationTests>();

        public override SettingsTask Project_for_coverage14() => base.Project_for_coverage14().UseSnapshotDirectoryAndNameOf<ObjectQueryIntegrationTests>();

        public override SettingsTask Project_for_coverage15() => base.Project_for_coverage15().UseSnapshotDirectoryAndNameOf<ObjectQueryIntegrationTests>();

        public override SettingsTask Project_to_tuple_maximum_expressions() => base.Project_to_tuple_maximum_expressions().UseSnapshotDirectoryAndNameOf<ObjectQueryIntegrationTests>();

        public override SettingsTask Project_to_type() => base.Project_to_type().UseSnapshotDirectoryAndNameOf<ObjectQueryIntegrationTests>();

        public override SettingsTask Project_to_type_with_identity() => base.Project_to_type_with_identity().UseSnapshotDirectoryAndNameOf<ObjectQueryIntegrationTests>();

        public override SettingsTask Project_to_type_without_explicit_identity() => base.Project_to_type_without_explicit_identity().UseSnapshotDirectoryAndNameOf<ObjectQueryIntegrationTests>();

        public override SettingsTask Project_to_type_with_struct() => base.Project_to_type_with_struct().UseSnapshotDirectoryAndNameOf<ObjectQueryIntegrationTests>();

        public override SettingsTask Project_with_cast() => base.Project_with_cast().UseSnapshotDirectoryAndNameOf<ObjectQueryIntegrationTests>();

        public override SettingsTask Properties_Meta() => base.Properties_Meta().UseSnapshotDirectoryAndNameOf<ObjectQueryIntegrationTests>();

        public override SettingsTask Properties_Meta_ValueMap() => base.Properties_Meta_ValueMap().UseSnapshotDirectoryAndNameOf<ObjectQueryIntegrationTests>();

        public override SettingsTask Properties_typed_no_parameters() => base.Properties_typed_no_parameters().UseSnapshotDirectoryAndNameOf<ObjectQueryIntegrationTests>();

        public override SettingsTask Properties_ValueMap_typed() => base.Properties_ValueMap_typed().UseSnapshotDirectoryAndNameOf<ObjectQueryIntegrationTests>();

        public override SettingsTask Properties_Values_typed() => base.Properties_Values_typed().UseSnapshotDirectoryAndNameOf<ObjectQueryIntegrationTests>();

        public override SettingsTask Tree_with_builder() => base.Tree_with_builder().UseSnapshotDirectoryAndNameOf<ObjectQueryIntegrationTests>();

        public override SettingsTask Tree_with_builder_and_modulators() => base.Tree_with_builder_and_modulators().UseSnapshotDirectoryAndNameOf<ObjectQueryIntegrationTests>();
        public override SettingsTask ValueMap_typed() => base.ValueMap_typed().UseSnapshotDirectoryAndNameOf<ObjectQueryIntegrationTests>();
    }
}
