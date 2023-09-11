using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;

namespace ExRam.Gremlinq.Providers.JanusGraph.Tests
{
    public sealed class IntegrationTests : QueryExecutionTest, IClassFixture<JanusGraphContainerFixture>
    {
        public new sealed class Verifier : ExecutingVerifier
        {
            private static readonly Regex RelationIdRegex = new("\"relationId\":[\\s]?\"[0-9a-z]{3}([-][0-9a-z]{3})*\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            public Verifier([CallerFilePath] string sourceFile = "") : base(sourceFile)
            {
            }

            protected override SettingsTask InnerVerify<T>(ValueTask<T> value) => base
                .InnerVerify(value)
                .ScrubRegex(RelationIdRegex, "\"relationId\": \"scrubbed\"");
        }

        public IntegrationTests(JanusGraphContainerFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new Verifier(),
            testOutputHelper)
        {
        }
    }
}
