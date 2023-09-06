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
        private static readonly Regex IdRegex = new("\"@value\":[\\s]*[\\d]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

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

            await base
                .InnerVerify(serialized)
                .ScrubRegex(IdRegex, "\"@value\": -1")
                .ScrubGuids();
        }
    }
}
