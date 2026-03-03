using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Providers.Core;
using System.Text.RegularExpressions;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public partial class MetaResponseIntegrationTests : QueryExecutionTest, IClassFixture<GremlinServerContainerFixture>
    {
        private static readonly Regex HostRegex = HostRegexImpl();

        private sealed class MetaResponseExecutingVerifier : ExecutingVerifier
        {
            public MetaResponseExecutingVerifier() : base()
            {

            }

            public override Task Verify<TElement>(IGremlinQueryBase<TElement> query) => base
                .Verify(query.Cast<MetaResponse<TElement>>());

            protected override SettingsTask ModifySettingsTask(SettingsTask task) => base
                .ModifySettingsTask(task)
                .ScrubRegex(HostRegex, "(host)");
        }

        public MetaResponseIntegrationTests(GremlinServerContainerFixture fixture) : base(
            fixture,
            new MetaResponseExecutingVerifier())
        {
        }

        [GeneratedRegex(@"/\d+\.\d+\.\d+\.\d+:\d+")]
        private static partial Regex HostRegexImpl();
    }
}
