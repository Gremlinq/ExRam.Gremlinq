using System.Collections.Immutable;
using System.Text.RegularExpressions;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Core.Transformation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Support.NewtonsoftJson.Tests
{
    public abstract class IntegrationTestFixture : GremlinqTestFixture
    {
        private static readonly Regex IdRegex = new("(\"id\"\\s*[:,]\\s*{\\s*\"@type\"\\s*:\\s*\"g:(Int32|Int64|UUID)\"\\s*,\\s*\"@value\":\\s*)([^\\s{}]+)(\\s*})", RegexOptions.IgnoreCase);

        protected IntegrationTestFixture(IGremlinQuerySource source) : base(source
            .ConfigureEnvironment(env => env
                .UseNewtonsoftJson()
                .ConfigureDeserializer(d => d
                    .Add(ConverterFactory
                        .Create<JToken, JToken>((token, env, recurse) => token)))))
        {
        }

        public override async Task Verify<TElement>(IGremlinQueryBase<TElement> query)
        {
            var serialized = JsonConvert.SerializeObject(
                await query
                    .Cast<JToken>()
                    .ToArrayAsync(),
                Formatting.Indented);

            var scrubbed = this
                .Scrubbers()
                .Aggregate(serialized, (s, func) => func(s));

            await GremlinqTestBase.Current.Verify(scrubbed);
        }

        protected override IImmutableList<Func<string, string>> Scrubbers() => base
            .Scrubbers()
            .Add(x => IdRegex.Replace(x, "$1-1$4"))
            .ScrubGuids();
    }
}
