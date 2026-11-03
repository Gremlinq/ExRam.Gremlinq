using System.Text.RegularExpressions;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.JanusGraph.Tests
{
    [IntegrationTest("Linux")]
    [IntegrationTest("Windows")]
    public class IntegrationTests : QueryExecutionTestsBase, IClassFixture<JanusGraphContainerFixture>
    {
        private static readonly Regex RelationIdRegex = new("\"relationId\":[\\s]?\"[0-9a-z]{3}([-][0-9a-z]{3})*\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public IntegrationTests(JanusGraphContainerFixture fixture) : base(
            fixture,
            new ExecutingVerifier().WithSettings(static task => task
                .ScrubLinesContaining("Traverser>")
                .ScrubRegex(RelationIdRegex, "\"relationId\": \"scrubbed\"")))
        {
        }
    }
}
