using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    [IntegrationTest("Linux")]
    [IntegrationTest("Windows")]
    public class IntegrationTests : QueryExecutionTest, IClassFixture<NeptuneContainerFixture>
    {
        public IntegrationTests(NeptuneContainerFixture fixture) : base(
            fixture,
            new ExecutingVerifier())
        {
        }
    }
}
