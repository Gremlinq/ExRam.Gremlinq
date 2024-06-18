using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;

namespace ExRam.Gremlinq.Providers.JanusGraph.Tests
{
    [IntegrationTest("Windows", "Linux")]
    public sealed class IntegrationTests : QueryExecutionTest, IClassFixture<JanusGraphContainerFixture>
    {
        public new sealed class Verifier : JTokenExecutingVerifier
        {
            private static readonly Regex RelationIdRegex = new("\"relationId\":[\\s]?\"[0-9a-z]{3}([-][0-9a-z]{3})*\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            public Verifier(Func<SettingsTask, SettingsTask>? settingsTaskModifier = null, [CallerFilePath] string sourceFile = "") : base(settingsTaskModifier, sourceFile)
            {
            }

            protected override SettingsTask InnerVerify<T>(ValueTask<T> value) => base
                .InnerVerify(value)
                .ScrubRegex(RelationIdRegex, "\"relationId\": \"scrubbed\"");
        }

        public IntegrationTests(JanusGraphContainerFixture fixture) : base(
            fixture,
            new Verifier())
        {
        }
    }
}
