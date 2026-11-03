using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class Graphson2BinaryMessageSerializationTests : Graphson2BinaryMessageSerializationTestsBase, IClassFixture<GremlinqFixture>, ISourceFileNameProvider<Graphson2BinaryMessageSerializationTests>
    {
        public Graphson2BinaryMessageSerializationTests(GremlinqFixture fixture) : base(fixture, new BinaryMessageSerializingVerifier<GraphSon2BinaryMessage>())
        {
        }

        public static string GetSourceFileName() => SourceFileName.OfThis();
    }
}
