using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTest"/> against Gremlin Server 3.8.2, verified against the answers of
    /// <see cref="IntegrationTests"/>. A fact that answers differently on 3.8 is overridden with its own answer until
    /// the tasks of #2529 remove the difference.
    /// </summary>
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public class IntegrationTests38 : QueryExecutionTest, IClassFixture<GremlinServer38ContainerFixture>, ISourceFileNameProvider<IntegrationTests38>
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

        public IntegrationTests38(GremlinServer38ContainerFixture fixture) : base(
            fixture,
            new Verifier())
        {
        }

        public static string GetSourceFileName() => SourceFileName.OfThis();
    }
}
