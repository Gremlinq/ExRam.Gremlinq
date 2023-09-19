using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class OuterProjectionTest : QueryExecutionTest
    {
        private sealed class ProjectionVerifier : GremlinQueryVerifier
        {
            public ProjectionVerifier(Func<SettingsTask, SettingsTask>? settingsTaskModifier = null, [CallerFilePath] string sourceFile = "") : base(settingsTaskModifier, sourceFile)
            {
            }

            public override SettingsTask Verify<TElement>(IGremlinQueryBase<TElement> query) => InnerVerify(query.ToTraversal().Projection.ToTraversal(query.AsAdmin().Environment).Steps.ToArray());
        }

        public OuterProjectionTest(ITestOutputHelper testOutputHelper) : base(GremlinqFixture.Empty, new ProjectionVerifier(), testOutputHelper)
        {
        }
    }
}
