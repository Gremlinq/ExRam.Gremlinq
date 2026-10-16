using System.Runtime.CompilerServices;

using ExRam.Gremlinq.Tests.Infrastructure;

using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class GremlinQueryVerifierTest : GremlinqTestBase
    {
        private sealed class NamingVerifier : GremlinQueryVerifier
        {
            public NamingVerifier([CallerFilePath] string sourceFile = "") : base(sourceFile)
            {
            }

            public override SettingsTask Verify<TElement>(IGremlinQueryBase<TElement> query) => InnerVerify(Task.FromResult(query.Debug()));

            protected override SettingsTask ModifySettingsTask(SettingsTask task) => base
                .ModifySettingsTask(task)
                .UseMethodName("Named_by_the_verifier");
        }

        public GremlinQueryVerifierTest() : base(new NamingVerifier())
        {
        }

        // Only a snapshot named by the test exists, so the test fails if the verifier's name wins.
        [Fact]
        public Task A_setting_chained_onto_Verify_applies_after_the_settings_of_the_verifier() => g
            .Inject(42)
            .Verify()
            .UseMethodName("Named_by_the_test");
    }
}
