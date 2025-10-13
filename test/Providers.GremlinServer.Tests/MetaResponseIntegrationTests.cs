using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public class MetaResponseIntegrationTests : QueryExecutionTest, IClassFixture<GremlinServerContainerFixture>
    {
        private sealed class MetaResponseExecutingVerifier : ExecutingVerifier
        {
            public override Task Verify<TElement>(IGremlinQueryBase<TElement> query) => base
                .Verify(query.Cast<MetaResponse<TElement>>());
        }

        public MetaResponseIntegrationTests(GremlinServerContainerFixture fixture) : base(
            fixture,
            new MetaResponseExecutingVerifier())
        {
        }
    }
}
