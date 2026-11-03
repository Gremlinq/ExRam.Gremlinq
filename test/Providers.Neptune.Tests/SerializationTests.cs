using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public sealed class SerializationTests : SerializationTestsBase, IClassFixture<NeptuneContainerFixture>, ISourceFileNameProvider<SerializationTests>
    {
        public SerializationTests(NeptuneContainerFixture fixture) : base(fixture, new SerializingVerifier<Bytecode>())
        {
        }

        public static string GetSourceFileName() => SourceFileName.OfThis();
    }
}
