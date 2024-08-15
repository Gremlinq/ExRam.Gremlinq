using System.Runtime.CompilerServices;

using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    [IntegrationTest("Windows", true)]
    public class IntegrationTests : QueryExecutionTest, IClassFixture<CosmosDbEmulatorFixture>
    {
        public class CosmosDbEmulatorExecutingVerifier : ExecutingVerifier
        {
            public CosmosDbEmulatorExecutingVerifier([CallerFilePath] string sourceFile = "") : base(sourceFile)
            {
            }

            protected override SettingsTask ModifySettingsTask(SettingsTask task) => base
                .ModifySettingsTask(task)
                .ScrubMember("x-ms-server-time-ms")
                .ScrubMember("x-ms-total-server-time-ms")
                .ScrubMember("x-ms-request-charge")
                .ScrubMember("x-ms-total-request-charge");
        }

        public IntegrationTests(CosmosDbEmulatorFixture fixture) : base(
            fixture,
            new CosmosDbEmulatorExecutingVerifier())
        {
        }

        [Fact(Skip = "id as key cannot be scrubbed.")]
        public override Task Group_with_key_identity()
        {
            return base.Group_with_key_identity();
        }
    }
}
