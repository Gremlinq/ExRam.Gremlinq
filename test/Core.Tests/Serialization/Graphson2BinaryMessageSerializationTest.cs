using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Core.Tests
{
    public class Graphson2BinaryMessageSerializationTest : QueryExecutionTest, IClassFixture<GremlinqFixture>, ISourceFileNameProvider<Graphson2BinaryMessageSerializationTest>
    {
        public Graphson2BinaryMessageSerializationTest(GremlinqFixture fixture) : base(
            fixture,
            new BinaryMessageSerializingVerifier<GraphSon2BinaryMessage>())
        {
        }

        public static string GetSourceFileName() => SourceFileName.OfThis();
    }
}
