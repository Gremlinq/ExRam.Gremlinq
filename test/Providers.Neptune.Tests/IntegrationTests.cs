using System.Collections.Immutable;
using System.Text.RegularExpressions;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Support.NewtonsoftJson.Tests;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public class IntegrationTests : QueryExecutionTest, IClassFixture<IntegrationTests.Fixture>
    {
        public new sealed class Fixture : ExecutingTestFixture
        {
            private static readonly Regex IdRegex1 = new("\"[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}([|]PartitionKey)?\"", RegexOptions.IgnoreCase);

            public Fixture() : base(Gremlinq.Core.GremlinQuerySource.g
                .UseNeptune(builder => builder
                    .AtLocalhost())
                .ConfigureEnvironment(environment => environment
                    .ConfigureExecutor(_ => _
                        .IgnoreResults())))
            {
            }

            protected override IImmutableList<Func<string, string>> Scrubbers() => base
                .Scrubbers()
                .Add(x => IdRegex1.Replace(x, "\"scrubbed id\""));
        }
        
        public IntegrationTests(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
