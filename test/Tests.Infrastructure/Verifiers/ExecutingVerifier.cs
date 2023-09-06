using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Transformation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public class ExecutingVerifier : GremlinQueryVerifier
    {
        private static readonly Regex IdRegex = new("(\"id\"\\s*[:,]\\s*{\\s*\"@type\"\\s*:\\s*\"g:(Int32|Int64|UUID)\"\\s*,\\s*\"@value\":\\s*)([^\\s{}]+)(\\s*})", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public ExecutingVerifier([CallerFilePath] string sourceFile = "") : base(sourceFile)
        {
        }

        public override async Task Verify<TElement>(IGremlinQueryBase<TElement> query)
        {
            var serialized = JsonConvert.SerializeObject(
                await query
                    .AsAdmin()
                    .ChangeQueryType<IGremlinQuerySource>()
                    .ConfigureEnvironment(env => env
                        .ConfigureDeserializer(d => d
                            .Add(ConverterFactory
                                .Create<JToken, JToken>((token, env, recurse) => token))))
                    .AsAdmin()
                    .ChangeQueryType<IGremlinQueryBase<JToken>>()
                    .ToAsyncEnumerable()
                    .Catch<JToken, GremlinQueryExecutionException>(ex => AsyncEnumerableEx
                        .Return<JToken>(new JObject()
                        {
                            {
                                "serverException",
                                new JObject
                                {
                                    { "type", ex.GetType().Name },
                                    { "message", ex.Message },
                                    {
                                        "innerException",
                                        new JObject
                                        {
                                            { "type", ex.InnerException?.GetType().Name },
                                            { "message", ex.InnerException?.Message },
                                        }
                                    }
                                }
                            }
                        }))
                    .ToArrayAsync(),
                Formatting.Indented);

            await InnerVerify(serialized);
        }

        protected override IImmutableList<Func<string, string>> Scrubbers() => base
            .Scrubbers()
            .Add(x => IdRegex.Replace(x, "$1-1$4"))
            .ScrubGuids();
    }
}
