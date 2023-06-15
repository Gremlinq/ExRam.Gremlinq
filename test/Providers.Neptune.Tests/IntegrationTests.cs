using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public sealed class IntegrationTests : QueryExecutionTest, IClassFixture<NeptuneFixture>
    {
        public IntegrationTests(NeptuneFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new ExecutingVerifier(),
            testOutputHelper)
        {
        }
    }
}
