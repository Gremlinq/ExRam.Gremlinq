using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class Graphson3BinaryMessageSerializationTests : Graphson3BinaryMessageSerializationTestsBase, IClassFixture<GremlinqFixture>, ISourceFileNameProvider<Graphson3BinaryMessageSerializationTests>
    {
        public Graphson3BinaryMessageSerializationTests(GremlinqFixture fixture) : base(fixture, new BinaryMessageSerializingVerifier<GraphSon3BinaryMessage>())
        {
        }

        public static string GetSourceFileName() => SourceFileName.OfThis();
    }
}
