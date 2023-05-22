using System.Collections.Immutable;
using System.Text.RegularExpressions;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Support.NewtonsoftJson.Tests;

namespace ExRam.Gremlinq.Providers.JanusGraph.Tests
{
    public class IntegrationTests : QueryExecutionTest, IClassFixture<IntegrationTests.Fixture>
    {
        public new sealed class Fixture : ExecutingTestFixture
        {
            private static readonly Regex RelationIdRegex = new("\"relationId\":[\\s]?\"[0-9a-z]{3}([-][0-9a-z]{3})*\"", RegexOptions.IgnoreCase);

            public Fixture() : base(Gremlinq.Core.GremlinQuerySource.g
                .UseJanusGraph(builder => builder
                    .At(new Uri("ws://localhost:8183"))
                    .UseNewtonsoftJson())
                .ConfigureEnvironment(environment => environment
                    .ConfigureExecutor(_ => _
                        .IgnoreResults())))
            {
            }

            protected override IImmutableList<Func<string, string>> Scrubbers() => base
                .Scrubbers()
                .Add(x => RelationIdRegex.Replace(x, "\"relationId\": \"scrubbed\""));
        }

        public IntegrationTests(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
