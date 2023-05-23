using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Support.NewtonsoftJson.Tests;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public sealed class IntegrationTests : QueryExecutionTest, IClassFixture<NeptuneFixture>
    {
        public new sealed class Verifier : ExecutingVerifier
        {
            private static readonly Regex IdRegex = new("\"[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}([|]PartitionKey)?\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            public Verifier([CallerFilePath] string sourceFile = "") : base(sourceFile)
            {
            }

            protected override IImmutableList<Func<string, string>> Scrubbers() => base
                .Scrubbers()
                .Add(x => IdRegex.Replace(x, "\"scrubbed id\""));
        }

        public IntegrationTests(NeptuneFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new Verifier(),
            testOutputHelper)
        {
        }
    }
}
