using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/> serialized to a request message by the Gremlin Server provider, like
    /// <see cref="Core.Tests.RequestMessageSerializationTests"/> of Core but for the facts the provider serializes differently, whose
    /// snapshots this class owns.
    /// </summary>
    public sealed class RequestMessageSerializationTests : RequestMessageSerializationTestsBase, IClassFixture<GremlinServerFixture>, ISourceFileNameProvider<RequestMessageSerializationTests>
    {
        public RequestMessageSerializationTests(GremlinServerFixture fixture) : base(fixture, new SerializingVerifier<RequestMessage>())
        {
        }

        public static string GetSourceFileName() => SourceFileName.OfThis();

        // On 3.7, limit(local, 1), tail(local, 1) and range(local, n, n + 1) return their single result bare instead of in a
        // list, so the Gremlin Server provider sends map(unfold().limit(1).fold()) and its like where Core sends the plain step:
        // https://tinkerpop.apache.org/docs/3.8.0/upgrade/#consistent-output-for-range-limit-tail
        public override SettingsTask Inject_Limit0_Fold_LimitLocal_1() => base.Inject_Limit0_Fold_LimitLocal_1().UseSnapshotDirectoryAndNameOf<RequestMessageSerializationTests>();

        public override SettingsTask Inject_Limit0_Fold_RangeLocal_1() => base.Inject_Limit0_Fold_RangeLocal_1().UseSnapshotDirectoryAndNameOf<RequestMessageSerializationTests>();

        public override SettingsTask Inject_Limit0_Fold_TailLocal_1() => base.Inject_Limit0_Fold_TailLocal_1().UseSnapshotDirectoryAndNameOf<RequestMessageSerializationTests>();

        public override SettingsTask Inject_Limit1_Fold_LimitLocal_1() => base.Inject_Limit1_Fold_LimitLocal_1().UseSnapshotDirectoryAndNameOf<RequestMessageSerializationTests>();

        public override SettingsTask Inject_Limit1_Fold_RangeLocal_1() => base.Inject_Limit1_Fold_RangeLocal_1().UseSnapshotDirectoryAndNameOf<RequestMessageSerializationTests>();

        public override SettingsTask Inject_Limit1_Fold_TailLocal_1() => base.Inject_Limit1_Fold_TailLocal_1().UseSnapshotDirectoryAndNameOf<RequestMessageSerializationTests>();

        public override SettingsTask Inject_Limit2_Fold_LimitLocal_1() => base.Inject_Limit2_Fold_LimitLocal_1().UseSnapshotDirectoryAndNameOf<RequestMessageSerializationTests>();

        public override SettingsTask Inject_Limit2_Fold_RangeLocal_1() => base.Inject_Limit2_Fold_RangeLocal_1().UseSnapshotDirectoryAndNameOf<RequestMessageSerializationTests>();

        public override SettingsTask Inject_Limit2_Fold_TailLocal_1() => base.Inject_Limit2_Fold_TailLocal_1().UseSnapshotDirectoryAndNameOf<RequestMessageSerializationTests>();

        public override SettingsTask LimitLocal() => base.LimitLocal().UseSnapshotDirectoryAndNameOf<RequestMessageSerializationTests>();

        public override SettingsTask TailLocal() => base.TailLocal().UseSnapshotDirectoryAndNameOf<RequestMessageSerializationTests>();
    }
}
