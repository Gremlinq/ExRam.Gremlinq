using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;

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
