using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/> answering like <see cref="IntegrationWithoutProjectionTests38"/>: Gremlin Server
    /// 3.8.2 without element projection. A fact that answers differently than on 3.7 verifies against the snapshot of
    /// <see cref="IntegrationTests38"/>, which is the same without projection too; every other fact answers like
    /// <see cref="IntegrationWithoutProjectionTests"/>. The in-memory provider of Gremlinq.Extensions derives from here.
    /// </summary>
    public abstract class IntegrationWithoutProjectionTests38Base : IntegrationWithoutProjectionTestsBase
    {
        protected IntegrationWithoutProjectionTests38Base(GremlinqFixture fixture, GremlinQueryVerifier verifier) : base(fixture, verifier)
        {
        }

        // 3.8's dateDiff() returns milliseconds instead of seconds:
        // https://tinkerpop.apache.org/docs/3.8.0/upgrade/#datediff-step-returns-milliseconds
        public override SettingsTask DateDiff_constant() => base.DateDiff_constant().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        public override SettingsTask DateDiff_constant_through_extension() => base.DateDiff_constant_through_extension().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        public override SettingsTask DateDiff_traversal() => base.DateDiff_traversal().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        public override SettingsTask DateDiff_traversal_through_extension() => base.DateDiff_traversal_through_extension().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        // 3.8's default date type is OffsetDateTime, so Gremlinq sends one to a server from 3.8 on:
        // https://tinkerpop.apache.org/docs/3.8.0/upgrade/#new-default-datetime-type
        // Read as a string, a stored date is the server's text of the OffsetDateTime instead of milliseconds since the Unix epoch.
        public override SettingsTask Properties_typed_no_parameters() => base.Properties_typed_no_parameters().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        public override SettingsTask Properties_ValueMap_typed() => base.Properties_ValueMap_typed().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        public override SettingsTask Properties_Values_typed() => base.Properties_Values_typed().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        // 3.8's asString() rejects a null instead of passing it on:
        // https://tinkerpop.apache.org/docs/3.8.0/upgrade/#asstring-no-longer-allow-nulls
        public override SettingsTask Concat_strings_to_null() => base.Concat_strings_to_null().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        // 3.7's choose() fails on a traverser for which the choice traversal yields nothing. 3.8 passes it through, and
        // Gremlinq fails on it with option(unproductive, fail(…)) instead, so this fact fails with another error than on 3.7:
        // https://tinkerpop.apache.org/docs/3.8.0/upgrade/#choose-semantics
        public override SettingsTask Choose_unproductive() => base.Choose_unproductive().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();
    }
}
