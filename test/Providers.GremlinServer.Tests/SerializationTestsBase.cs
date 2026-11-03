using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/> serialized to bytecode by the Gremlin Server provider for a server below
    /// TinkerPop 3.8, answering like <see cref="SerializationTests"/>: like <see cref="BytecodeQuerySerializationTests"/> of Core, but
    /// for the facts the provider serializes differently, which verify against the snapshot of <see cref="SerializationTests"/>, the
    /// leaf that owns them.
    /// </summary>
    public abstract class SerializationTestsBase : BytecodeQuerySerializationTestsBase
    {
        protected SerializationTestsBase(GremlinqFixture fixture, GremlinQueryVerifier verifier) : base(fixture, verifier)
        {
        }

        // On 3.7, limit(local, 1), tail(local, 1) and range(local, n, n + 1) return their single result bare instead of in a
        // list, so the Gremlin Server provider sends map(unfold().limit(1).fold()) and its like where Core sends the plain step:
        // https://tinkerpop.apache.org/docs/3.8.0/upgrade/#consistent-output-for-range-limit-tail
        public override SettingsTask Inject_Limit0_Fold_LimitLocal_1() => base.Inject_Limit0_Fold_LimitLocal_1().UseSnapshotDirectoryAndNameOf<SerializationTests>();

        public override SettingsTask Inject_Limit0_Fold_RangeLocal_1() => base.Inject_Limit0_Fold_RangeLocal_1().UseSnapshotDirectoryAndNameOf<SerializationTests>();

        public override SettingsTask Inject_Limit0_Fold_TailLocal_1() => base.Inject_Limit0_Fold_TailLocal_1().UseSnapshotDirectoryAndNameOf<SerializationTests>();

        public override SettingsTask Inject_Limit1_Fold_LimitLocal_1() => base.Inject_Limit1_Fold_LimitLocal_1().UseSnapshotDirectoryAndNameOf<SerializationTests>();

        public override SettingsTask Inject_Limit1_Fold_RangeLocal_1() => base.Inject_Limit1_Fold_RangeLocal_1().UseSnapshotDirectoryAndNameOf<SerializationTests>();

        public override SettingsTask Inject_Limit1_Fold_TailLocal_1() => base.Inject_Limit1_Fold_TailLocal_1().UseSnapshotDirectoryAndNameOf<SerializationTests>();

        public override SettingsTask Inject_Limit2_Fold_LimitLocal_1() => base.Inject_Limit2_Fold_LimitLocal_1().UseSnapshotDirectoryAndNameOf<SerializationTests>();

        public override SettingsTask Inject_Limit2_Fold_RangeLocal_1() => base.Inject_Limit2_Fold_RangeLocal_1().UseSnapshotDirectoryAndNameOf<SerializationTests>();

        public override SettingsTask Inject_Limit2_Fold_TailLocal_1() => base.Inject_Limit2_Fold_TailLocal_1().UseSnapshotDirectoryAndNameOf<SerializationTests>();

        public override SettingsTask LimitLocal() => base.LimitLocal().UseSnapshotDirectoryAndNameOf<SerializationTests>();

        public override SettingsTask TailLocal() => base.TailLocal().UseSnapshotDirectoryAndNameOf<SerializationTests>();
    }
}
