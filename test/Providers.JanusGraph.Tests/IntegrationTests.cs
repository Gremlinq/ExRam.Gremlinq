using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Support.NewtonsoftJson.Tests.Verifier;

namespace ExRam.Gremlinq.Providers.JanusGraph.Tests
{
    public sealed class IntegrationTests : QueryExecutionTest, IClassFixture<IntegrationJanusGraphFixture>
    {
        public new sealed class Verifier : ExecutingVerifier
        {
            private static readonly Regex RelationIdRegex = new("\"relationId\":[\\s]?\"[0-9a-z]{3}([-][0-9a-z]{3})*\"", RegexOptions.IgnoreCase);

            public Verifier([CallerFilePath] string sourceFile = "") : base(sourceFile)
            {
            }

            protected override IImmutableList<Func<string, string>> Scrubbers() => base
                .Scrubbers()
                .Add(x => RelationIdRegex.Replace(x, "\"relationId\": \"scrubbed\""));
        }

        public IntegrationTests(IntegrationJanusGraphFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new Verifier(),
            testOutputHelper)
        {
        }
    }
}
