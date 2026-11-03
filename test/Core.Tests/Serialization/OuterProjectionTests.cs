using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class OuterProjectionTests : QueryExecutionTestsBase, IClassFixture<GremlinqFixture>
    {
        private sealed class ProjectionVerifier : GremlinQueryVerifier
        {
            public ProjectionVerifier([CallerFilePath] string sourceFile = "") : base(sourceFile)
            {
            }

            public override SettingsTask Verify<TElement>(IGremlinQueryBase<TElement> query) => InnerVerify(query.ToTraversal().Projection.ToTraversal(query.AsAdmin().Environment).Steps.ToArray());
        }

        public OuterProjectionTests(GremlinqFixture fixture) : base(fixture, new ProjectionVerifier())
        {
        }
    }
}
