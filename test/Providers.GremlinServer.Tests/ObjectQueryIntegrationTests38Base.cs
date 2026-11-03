using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/> answering like <see cref="ObjectQueryIntegrationTests38"/>: every query cast to
    /// <see cref="object"/>, against Gremlin Server 3.8.2. A fact that answers differently than on 3.7 verifies against the
    /// snapshot of <see cref="IntegrationTests38"/>, which is the same; every other fact answers like
    /// <see cref="ObjectQueryIntegrationTests"/>.
    /// </summary>
    public abstract class ObjectQueryIntegrationTests38Base : ObjectQueryIntegrationTestsBase
    {
        protected ObjectQueryIntegrationTests38Base(GremlinqFixture fixture, GremlinQueryVerifier verifier) : base(fixture, verifier)
        {
        }

        // 3.8's dateDiff() returns milliseconds instead of seconds. Gremlinq does not make up for it, so these
        // overrides stay: https://tinkerpop.apache.org/docs/3.8.0/upgrade/#datediff-step-returns-milliseconds
        public override SettingsTask DateDiff_constant() => base.DateDiff_constant().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        public override SettingsTask DateDiff_constant_through_extension() => base.DateDiff_constant_through_extension().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        public override SettingsTask DateDiff_traversal() => base.DateDiff_traversal().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        public override SettingsTask DateDiff_traversal_through_extension() => base.DateDiff_traversal_through_extension().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        // The same groups as on 3.7, in another order. Gremlinq keeps the order in which the server sends the map of
        // group(), and the server's order changes once the stored dates are OffsetDateTime:
        // https://tinkerpop.apache.org/docs/3.8.0/upgrade/#new-default-datetime-type
        public override SettingsTask Group() => base.Group().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        public override SettingsTask Group_with_key_identity() => base.Group_with_key_identity().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        // 3.8's asString() rejects a null instead of passing it on. Gremlinq does not make up for it, so this
        // override stays: https://tinkerpop.apache.org/docs/3.8.0/upgrade/#asstring-no-longer-allow-nulls
        public override SettingsTask Concat_strings_to_null() => base.Concat_strings_to_null().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();

        // 3.7's choose() fails on a traverser for which the choice traversal yields nothing. 3.8 passes it through, and
        // Gremlinq fails on it with option(unproductive, fail(…)) instead, so this fact fails with another error than on 3.7:
        // https://tinkerpop.apache.org/docs/3.8.0/upgrade/#choose-semantics
        public override SettingsTask Choose_unproductive() => base.Choose_unproductive().UseSnapshotDirectoryAndNameOf<IntegrationTests38>();
    }
}
