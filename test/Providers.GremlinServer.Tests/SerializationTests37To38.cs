using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTest"/>, serialized for a server that understands TinkerPop 3.7.7 to 3.8.2 and verified
    /// against the answers of <see cref="SerializationTests"/>. Such a server understands every form before 3.8, so no fact differs.
    /// </summary>
    public class SerializationTests37To38 : QueryExecutionTest, IClassFixture<GremlinServer37To38ContainerFixture>
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

        public SerializationTests37To38(GremlinServer37To38ContainerFixture fixture) : base(
            fixture,
            new Verifier())
        {
        }
    }
}
