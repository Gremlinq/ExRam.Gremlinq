using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Core.Tests
{
    public class Graphson3BinaryMessageSerializationTest : QueryExecutionTest, IClassFixture<GremlinqFixture>, ISourceFileNameProvider<Graphson3BinaryMessageSerializationTest>
    {
        public Graphson3BinaryMessageSerializationTest(GremlinqFixture fixture) : base(
            fixture,
            new BinaryMessageSerializingVerifier<GraphSon3BinaryMessage>())
        {
        }

        public static string GetSourceFileName() => SourceFileName.OfThis();

        [Fact]
        public Task MaxDepth() => _g
            .Inject(0)
            .Map(GetLambda(29))
            .Verify();

        private Func<IGremlinQuery<int>, IGremlinQuery<int>> GetLambda(int i) => i == 0
            ? __ => __.Constant(1)
            : __ => __.Map(GetLambda(i - 1));
    }
}
