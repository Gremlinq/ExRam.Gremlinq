using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;

namespace ExRam.Gremlinq.Core.Tests
{
    public class OuterProjectionTest : QueryExecutionTest, IClassFixture<GremlinqFixture>
    {
        private sealed class ProjectionVerifier : GremlinQueryVerifier
        {
            public ProjectionVerifier([CallerFilePath] string sourceFile = "") : base(sourceFile)
            {
            }

            public override Task Verify<TElement>(IGremlinQueryBase<TElement> query) => InnerVerify(query.ToTraversal().Projection.ToTraversal(query.AsAdmin().Environment).Steps.ToArray());
        }

        public OuterProjectionTest(GremlinqFixture fixture, ITestOutputHelper testOutputHelper) : base(fixture, new ProjectionVerifier())
        {
        }
    }
}
