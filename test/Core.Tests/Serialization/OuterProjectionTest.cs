using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class OuterProjectionTest : QueryExecutionTest, IClassFixture<EmptyGremlinqTestFixture>
    {
        private sealed class ProjectionVerifier : GremlinQueryVerifier
        {
            public ProjectionVerifier(Func<SettingsTask, SettingsTask>? settingsTaskModifier = null, [CallerFilePath] string sourceFile = "") : base(settingsTaskModifier, sourceFile)
            {
            }

            public override SettingsTask Verify<TElement>(IGremlinQueryBase<TElement> query) => InnerVerify(query.ToTraversal().Projection.ToTraversal(query.AsAdmin().Environment).Steps.ToArray());
        }

        public OuterProjectionTest(EmptyGremlinqTestFixture fixture, ITestOutputHelper testOutputHelper) : base(fixture, new ProjectionVerifier(), testOutputHelper)
        {
        }
    }
}
