using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    [IntegrationTest("Linux", false)]
    [IntegrationTest("Windows", false)]
    public sealed class IntegrationTests : QueryExecutionTest, IClassFixture<NeptuneContainerFixture>
    {
        public IntegrationTests(NeptuneContainerFixture fixture) : base(
            fixture,
            new JTokenExecutingVerifier())
        {
        }
    }
}
