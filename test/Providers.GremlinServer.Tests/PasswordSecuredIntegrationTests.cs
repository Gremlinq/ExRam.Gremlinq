using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public class PasswordSecuredIntegrationTests : QueryExecutionTest, IClassFixture<PasswordSecuredGremlinServerContainerFixture>
    {
        public new class Verifier : ExecutingVerifier
        {
            public Verifier() : base()
            {

            }

            protected override SettingsTask ModifySettingsTask(SettingsTask task) => base
                .ModifySettingsTask(task)
                .UseSnapshotDirectoryAndNameOf<IntegrationTests>();
        }

        public PasswordSecuredIntegrationTests(PasswordSecuredGremlinServerContainerFixture fixture) : base(
            fixture,
            new Verifier())
        {
        }
    }
}
