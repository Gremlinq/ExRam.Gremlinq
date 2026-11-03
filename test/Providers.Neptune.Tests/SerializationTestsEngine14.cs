using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/> serialized to bytecode by the Neptune provider for engine 1.4.8.1, which
    /// understands TinkerPop 3.7.1 only, so every fact answers like <see cref="SerializationTests"/>.
    /// </summary>
    public sealed class SerializationTestsEngine14 : SerializationTestsBase, IClassFixture<SerializationTestsEngine14.Fixture>
    {
        public sealed class Fixture : NeptuneContainerFixture
        {
            public Fixture() : base(TinkerPopVersions.V3_7_7, NeptuneEngineVersions.V1_4_8_1)
            {
            }
        }

        public SerializationTestsEngine14(Fixture fixture) : base(fixture, new SerializingVerifier<Bytecode>())
        {
        }
    }
}
