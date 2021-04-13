#if RELEASE && NET5_0 && RUNNEPTUNEINTEGRATIONTESTS
using ExRam.Gremlinq.Core.Tests;
using Xunit;
using Xunit.Abstractions;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public class NeptuneIntegrationTests : QueryIntegrationTest, IClassFixture<NeptuneFixture>
    {
        public NeptuneIntegrationTests(NeptuneFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
#endif
