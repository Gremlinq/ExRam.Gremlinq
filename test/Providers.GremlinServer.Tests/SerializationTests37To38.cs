using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/> serialized to bytecode by the Gremlin Server provider for a server that
    /// understands TinkerPop 3.7.7 to 3.8.2. Such a server understands every form before 3.8, so every fact answers like
    /// <see cref="SerializationTests"/>.
    /// </summary>
    public sealed class SerializationTests37To38 : SerializationTestsBase, IClassFixture<SerializationTests37To38.Fixture>
    {
        public sealed class Fixture : GremlinServerFixture
        {
            protected override IGremlinServerConfigurator ConfigureProvider(IGremlinServerConfigurator configurator) => base
                .ConfigureProvider(configurator)
                .ConfigureServerCompatibilityRange(_ => VersionRange.From(TinkerPopVersions.V3_7_7, TinkerPopVersions.V3_8_2));
        }

        public SerializationTests37To38(Fixture fixture) : base(fixture, new SerializingVerifier<Bytecode>())
        {
        }
    }
}
