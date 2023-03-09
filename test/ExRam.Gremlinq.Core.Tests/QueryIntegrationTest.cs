using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ExRam.Gremlinq.Core.Deserialization;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Transformation;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class QueryIntegrationTest : QueryExecutionTest
    {
        public abstract class Fixture : GremlinqTestFixture
        {
            protected Fixture(IGremlinQuerySource source) : base(source
                .ConfigureEnvironment(env => env
                    .ConfigureExecutor(_ => _
                        .TransformResult(enumerable => enumerable
                            .Catch<object, Exception>(ex => AsyncEnumerableEx
                                .Return<object>(new JObject()
                                {
                                    {
                                        "serverException",
                                        new JObject
                                        {
                                            { "type", ex.GetType().Name },
                                            { "message", ex.Message }
                                        }
                                    }
                                }))))
                    .ConfigureDeserializer(d => d
                        .AddNewtonsoftJson()
                        .Add(ConverterFactory
                            .Create<JToken, JToken[]>((token, env, recurse) => new[] { token })))))
            {
            }
        }

        private static readonly Regex IdRegex = new ("(\"id\"\\s*[:,]\\s*{\\s*\"@type\"\\s*:\\s*\"g:(Int32|Int64|UUID)\"\\s*,\\s*\"@value\":\\s*)([^\\s{}]+)(\\s*})", RegexOptions.IgnoreCase);

        protected QueryIntegrationTest(Fixture fixture, ITestOutputHelper testOutputHelper, [CallerFilePath] string callerFilePath = "") : base(
            fixture,
            testOutputHelper,
            callerFilePath)
        {
        }

        public override Task Verify<TElement>(IGremlinQueryBase<TElement> query) => base.Verify(query.Cast<JToken>());

        protected override IImmutableList<Func<string, string>> Scrubbers() => base.Scrubbers()
            .Add(x => IdRegex.Replace(x, "$1-1$4"))
            .ScrubGuids();
    }
}
