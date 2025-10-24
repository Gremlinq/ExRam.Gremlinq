using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    [IntegrationTest("Windows", true)]
    public partial class MetaResponseIntegrationTests : CosmosDbIntegrationTestBase, IClassFixture<CosmosDbEmulatorFixture>
    {
        private sealed class MetaResponseExecutingVerifier : ExecutingVerifier
        {
            public MetaResponseExecutingVerifier() : base()
            {

            }

            public override Task Verify<TElement>(IGremlinQueryBase<TElement> query) => base
                .Verify(query.Cast<MetaResponse<TElement>>());

            protected override SettingsTask ModifySettingsTask(SettingsTask task) => base
                .ModifySettingsTask(task)
                .IgnoreMember("Attributes")
                .ScrubMember("x-ms-server-time-ms")
                .ScrubMember("x-ms-total-server-time-ms")
                .ScrubMember("x-ms-request-charge")
                .ScrubMember("x-ms-total-request-charge");
        }

        public MetaResponseIntegrationTests(CosmosDbEmulatorFixture fixture) : base(
            fixture,
            new MetaResponseExecutingVerifier())
        {
        }
    }
}
