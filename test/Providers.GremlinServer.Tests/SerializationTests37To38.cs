using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/>, serialized for a server that understands TinkerPop 3.7.7 to 3.8.2 and verified
    /// against the answers of <see cref="SerializationTests"/>. Such a server understands every form before 3.8, so no fact differs.
    /// </summary>
    public class SerializationTests37To38 : QueryExecutionTestsBase, IClassFixture<SerializationTests37To38.Fixture>
    {
        public sealed class Fixture : GremlinServerContainerFixture
        {
            public Fixture() : base(TinkerPopVersions.V3_8_2, VersionRange.From(TinkerPopVersions.V3_7_7, TinkerPopVersions.V3_8_2))
            {
            }
        }

        public SerializationTests37To38(Fixture fixture) : base(
            fixture,
            new SerializingVerifier<Bytecode>().WithSnapshotsOf<SerializationTests>())
        {
        }
    }
}
