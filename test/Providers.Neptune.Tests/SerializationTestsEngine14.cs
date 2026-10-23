using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTest"/>, serialized for Neptune engine 1.4.8.1 and verified against the answers of
    /// <see cref="SerializationTests"/>. That engine understands TinkerPop 3.7.1 only, so no fact differs.
    /// </summary>
    public class SerializationTestsEngine14 : QueryExecutionTest, IClassFixture<NeptuneEngine14ContainerFixture>
    {
        public new class Verifier : SerializingVerifier<Bytecode>
        {
            public Verifier() : base()
            {

            }

            protected override SettingsTask ModifySettingsTask(SettingsTask task) => base
                .ModifySettingsTask(task)
                .UseSnapshotDirectoryAndNameOf<SerializationTests>();
        }

        public SerializationTestsEngine14(NeptuneEngine14ContainerFixture fixture) : base(
            fixture,
            new Verifier())
        {
        }
    }
}
