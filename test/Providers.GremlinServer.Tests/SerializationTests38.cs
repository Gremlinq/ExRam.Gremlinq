using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/> serialized to bytecode by the Gremlin Server provider for TinkerPop 3.8.2 only.
    /// Owns the snapshots of the facts whose query 3.8 needs in another form; <see cref="SerializationTests38Base"/> names them.
    /// </summary>
    public sealed class SerializationTests38 : SerializationTests38Base, IClassFixture<SerializationTests38.Fixture>, ISourceFileNameProvider<SerializationTests38>
    {
        public sealed class Fixture : GremlinServerFixture
        {
            protected override IGremlinServerConfigurator ConfigureProvider(IGremlinServerConfigurator configurator) => base
                .ConfigureProvider(configurator)
                .ConfigureServerCompatibilityRange(_ => VersionRange.From(TinkerPopVersions.V3_8_2, TinkerPopVersions.V3_8_2));
        }

        public SerializationTests38(Fixture fixture) : base(fixture, new SerializingVerifier<Bytecode>())
        {
        }

        public static string GetSourceFileName() => SourceFileName.OfThis();
    }
}
