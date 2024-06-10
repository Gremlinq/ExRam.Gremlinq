using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.TestCases;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class Graphson3BinaryMessageSerializationTest : QueryExecutionTest, IClassFixture<GremlinqFixture>
    {
        public Graphson3BinaryMessageSerializationTest(GremlinqFixture fixture) : base(
            fixture,
            new BinaryMessageSerializingVerifier<GraphSon3BinaryMessage>())
        {
        }


        [Fact]
        public Task MaxDepth()
        {
            return _g
                .Inject(0)
                .Map(GetLambda(29))
                .Verify();
        }

        private Func<IGremlinQuery<int>, IGremlinQuery<int>> GetLambda(int i)
        {
            return i == 0
                ? __ => __.Constant(1)
                : __ => __.Map(GetLambda(i - 1));
        }
    }
}
