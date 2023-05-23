using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Support.NewtonsoftJson.Tests.Verifier;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public sealed class IntegrationTests : QueryExecutionTest, IClassFixture<IntegrationTests.Fixture>
    {
        public new sealed class Verifier : ExecutingVerifier
        {
            private static readonly Regex IdRegex1 = new("\"[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}([|]PartitionKey)?\"", RegexOptions.IgnoreCase);

            public Verifier([CallerFilePath] string sourceFile = "") : base(sourceFile)
            {
            }

            protected override IImmutableList<Func<string, string>> Scrubbers() => base
                .Scrubbers()
                .Add(x => IdRegex1.Replace(x, "\"scrubbed id\""));
        }

        public sealed class Fixture : GremlinqTestFixture
        {
            public Fixture() : base(Gremlinq.Core.GremlinQuerySource.g
                .UseNeptune(builder => builder
                    .AtLocalhost())
                .ConfigureEnvironment(env => env
                    .ConfigureDeserializer(d => d
                        .Add(ConverterFactory
                            .Create<JToken, JTokenExecutionResult>((token, env, recurse) => new JTokenExecutionResult(token)))))
                .ConfigureEnvironment(environment => environment
                    .ConfigureExecutor(_ => _
                        .IgnoreResults())))
            {
            }
        }
        
        public IntegrationTests(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new Verifier(),
            testOutputHelper)
        {
        }
    }
}
