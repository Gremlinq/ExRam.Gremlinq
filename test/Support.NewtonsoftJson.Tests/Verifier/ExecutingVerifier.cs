using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Support.NewtonsoftJson.Tests.Verifier
{
    public readonly struct JTokenExecutionResult
    {
        private readonly JToken? _result;

        public JTokenExecutionResult(JToken result)
        {
            _result = result;
        }

        public JToken Result => _result ?? throw new InvalidOperationException();
    }

    public class ExecutingVerifier : GremlinQueryVerifier
    {
        private static readonly Regex IdRegex = new("(\"id\"\\s*[:,]\\s*{\\s*\"@type\"\\s*:\\s*\"g:(Int32|Int64|UUID)\"\\s*,\\s*\"@value\":\\s*)([^\\s{}]+)(\\s*})", RegexOptions.IgnoreCase);

        public ExecutingVerifier([CallerFilePath] string sourceFile = "") : base(sourceFile)
        {
        }

        public override async Task Verify<TElement>(IGremlinQueryBase<TElement> query)
        {
            var serialized = JsonConvert.SerializeObject(
                await query
                    .Cast<JTokenExecutionResult>()
                    .ToArrayAsync(),
                Formatting.Indented);

            var scrubbed = 
                Scrubbers()
                .Aggregate(serialized, (s, func) => func(s));

            await InnerVerify(scrubbed);
        }

        protected override IImmutableList<Func<string, string>> Scrubbers() => base
            .Scrubbers()
            .Add(x => IdRegex.Replace(x, "$1-1$4"))
            .ScrubGuids();
    }
}
