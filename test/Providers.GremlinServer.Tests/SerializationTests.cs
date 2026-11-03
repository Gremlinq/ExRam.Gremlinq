using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/> serialized to bytecode by the Gremlin Server provider for a server below
    /// TinkerPop 3.8. Owns the snapshots of the facts the provider serializes differently than Core; <see cref="SerializationTestsBase"/>
    /// names them.
    /// </summary>
    public sealed class SerializationTests : SerializationTestsBase, IClassFixture<GremlinServerFixture>, ISourceFileNameProvider<SerializationTests>
    {
        public SerializationTests(GremlinServerFixture fixture) : base(fixture, new SerializingVerifier<Bytecode>())
        {
        }

        public static string GetSourceFileName() => SourceFileName.OfThis();
    }
}
