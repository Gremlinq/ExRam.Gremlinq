using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class OuterProjectionTest : QueryExecutionTest
    {
        private sealed class ProjectionVerifier : GremlinQueryVerifier
        {
            public override Task Verify<TElement>(IGremlinQueryBase<TElement> query) => InnerVerify(query.ToTraversal().Projection);
        }

        public OuterProjectionTest(ITestOutputHelper testOutputHelper) : base(GremlinqFixture.Empty, new ProjectionVerifier(), testOutputHelper)
        {
        }
    }
}
