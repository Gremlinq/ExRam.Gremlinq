using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTest"/>, sent as Groovy scripts, against the customized Gremlin Server 3.8.2,
    /// verified against the answers of <see cref="GroovyIntegrationTests"/>. A fact that answers differently on 3.8 is
    /// overridden and verified against the answer of <see cref="IntegrationTests38"/> where it is the same, with its own
    /// answer otherwise.
    /// </summary>
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public class GroovyIntegrationTests38 : QueryExecutionTest, IClassFixture<GroovyIntegrationTests38.Fixture>, ISourceFileNameProvider<GroovyIntegrationTests38>
    {
        public class Fixture : GroovyIntegrationTests.Fixture
        {
            public Fixture() : base(TinkerPopVersions.V3_8_2)
            {
            }
        }

        public new class Verifier : ExecutingVerifier
        {
            public Verifier() : base()
            {

            }

            protected override SettingsTask ModifySettingsTask(SettingsTask task) => base
                .ModifySettingsTask(task)
                .UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests>();
        }

        public GroovyIntegrationTests38(Fixture fixture) : base(
            fixture,
            new Verifier())
        {
        }

        public static string GetSourceFileName() => SourceFileName.OfThis();

        // 3.8's dateDiff() returns milliseconds instead of seconds. Gremlinq does not make up for it, so these
        // overrides stay: https://tinkerpop.apache.org/docs/3.8.0/upgrade/#datediff-step-returns-milliseconds
        public override SettingsTask DateDiff_constant() => base.DateDiff_constant().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        public override SettingsTask DateDiff_constant_through_extension() => base.DateDiff_constant_through_extension().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        public override SettingsTask DateDiff_traversal() => base.DateDiff_traversal().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        public override SettingsTask DateDiff_traversal_through_extension() => base.DateDiff_traversal_through_extension().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        // 3.8's default date type is OffsetDateTime, so Gremlinq sends one to a server from 3.8 on:
        // https://tinkerpop.apache.org/docs/3.8.0/upgrade/#new-default-datetime-type
        // Read as a string, a stored date is the server's text of the OffsetDateTime instead of milliseconds since the Unix epoch.
        public override SettingsTask Properties_typed_no_parameters() => base.Properties_typed_no_parameters().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        public override SettingsTask Properties_ValueMap_typed() => base.Properties_ValueMap_typed().UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests38>();

        public override SettingsTask Properties_Values_typed() => base.Properties_Values_typed().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        // The same groups as on 3.7, in another order. Gremlinq keeps the order in which the server sends the map of
        // group(), and the server's order changes once the stored dates are OffsetDateTime.
        public override SettingsTask Group() => base.Group().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        public override SettingsTask Group_with_key_identity() => base.Group_with_key_identity().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        // 3.8's asString() rejects a null instead of passing it on. Gremlinq does not make up for it, so this
        // override stays: https://tinkerpop.apache.org/docs/3.8.0/upgrade/#asstring-no-longer-allow-nulls
        public override SettingsTask Concat_strings_to_null() => base.Concat_strings_to_null().UseSnapshotDirectoryAndNameOf<GroovyIntegrationTests38>();

        // 3.7's choose() fails on a traverser for which the choice traversal yields nothing. 3.8 passes it through, and
        // Gremlinq fails on it with option(unproductive, fail(…)) instead, so this fact fails with another error than on 3.7:
        // https://tinkerpop.apache.org/docs/3.8.0/upgrade/#choose-semantics
        public override SettingsTask Choose_unproductive() => base.Choose_unproductive().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();
    }
}
