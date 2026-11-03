using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/> serialized to bytecode by the Neptune provider for engine 1.5.0.0. Owns the
    /// snapshots of the facts whose query TinkerPop 3.8 needs in another form; <see cref="SerializationTestsEngine15Base"/> names them.
    /// </summary>
    public sealed class SerializationTestsEngine15 : SerializationTestsEngine15Base, IClassFixture<SerializationTestsEngine15.Fixture>, ISourceFileNameProvider<SerializationTestsEngine15>
    {
        public sealed class Fixture : NeptuneContainerFixture
        {
            public Fixture() : base(TinkerPopVersions.V3_8_2, NeptuneEngineVersions.V1_5_0_0)
            {
            }
        }

        public SerializationTestsEngine15(Fixture fixture) : base(fixture, new SerializingVerifier<Bytecode>())
        {
        }

        public static string GetSourceFileName() => SourceFileName.OfThis();
    }
}
