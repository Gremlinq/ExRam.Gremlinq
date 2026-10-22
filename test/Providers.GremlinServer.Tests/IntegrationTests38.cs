using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTest"/> against Gremlin Server 3.8.2, verified against the answers of
    /// <see cref="IntegrationTests"/>. A fact that answers differently on 3.8 is overridden with its own answer until
    /// the tasks of #2529 remove the difference.
    /// </summary>
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public class IntegrationTests38 : QueryExecutionTest, IClassFixture<GremlinServer38ContainerFixture>, ISourceFileNameProvider<IntegrationTests38>
    {
        public new class Verifier : ExecutingVerifier
        {
            public Verifier() : base()
            {

            }

            protected override SettingsTask ModifySettingsTask(SettingsTask task) => base
                .ModifySettingsTask(task)
                .UseSnapshotDirectoryAndNameOf<IntegrationTests>();
        }

        public IntegrationTests38(GremlinServer38ContainerFixture fixture) : base(
            fixture,
            new Verifier())
        {
        }

        public static string GetSourceFileName() => SourceFileName.OfThis();

        // 3.8 removed has(key, traversal). The server still accepts it, but the traversal never matches:
        // https://tinkerpop.apache.org/docs/3.8.0/upgrade/#removal-of-has-key-traversal
        public override SettingsTask Where_property_array_contains_stepLabel() => base.Where_property_array_contains_stepLabel().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        public override SettingsTask Where_property_array_intersects_stepLabel1() => base.Where_property_array_intersects_stepLabel1().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        public override SettingsTask Where_property_array_intersects_stepLabel2() => base.Where_property_array_intersects_stepLabel2().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        public override SettingsTask Where_property_equals_stepLabel() => base.Where_property_equals_stepLabel().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        public override SettingsTask Where_property_is_greater_than_or_equal_stepLabel() => base.Where_property_is_greater_than_or_equal_stepLabel().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        public override SettingsTask Where_property_is_lower_than_or_equal_stepLabel() => base.Where_property_is_lower_than_or_equal_stepLabel().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        public override SettingsTask Where_property_traversal() => base.Where_property_traversal().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        // 3.8 removed aggregate(local, …), the server knows no such method:
        // https://tinkerpop.apache.org/docs/3.8.0/upgrade/#removal-of-aggregate-with-scope-and-store
        public override SettingsTask Aggregate_Local() => base.Aggregate_Local().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        public override SettingsTask Aggregate_Local_with_existing_step() => base.Aggregate_Local_with_existing_step().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        // 3.8's dateDiff() returns milliseconds instead of seconds:
        // https://tinkerpop.apache.org/docs/3.8.0/upgrade/#datediff-step-returns-milliseconds
        public override SettingsTask DateDiff_constant() => base.DateDiff_constant().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        public override SettingsTask DateDiff_constant_through_extension() => base.DateDiff_constant_through_extension().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        public override SettingsTask DateDiff_traversal() => base.DateDiff_traversal().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        public override SettingsTask DateDiff_traversal_through_extension() => base.DateDiff_traversal_through_extension().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        // 3.8's asString() rejects a null instead of passing it on. Gremlinq does not make up for it, so this
        // override stays: https://tinkerpop.apache.org/docs/3.8.0/upgrade/#asstring-no-longer-allow-nulls
        public override SettingsTask Concat_strings_to_null() => base.Concat_strings_to_null().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();
    }
}
