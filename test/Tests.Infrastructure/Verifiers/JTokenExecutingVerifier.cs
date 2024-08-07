using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Transformation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public class JTokenExecutingVerifier : GremlinQueryVerifier
    {
        private static readonly ConditionalWeakTable<IGremlinQueryEnvironment, IGremlinQueryEnvironment> _environments = new();

        public JTokenExecutingVerifier([CallerFilePath] string sourceFile = "") : base(sourceFile)
        {
        }

        public override async Task Verify<TElement>(IGremlinQueryBase<TElement> query) => await InnerVerify(JsonConvert.SerializeObject(
            await query
                .AsAdmin()
                .ChangeQueryType<IGremlinQuerySource>()
                .ConfigureEnvironment(env => _environments
                    .GetValue(
                        env,
                        static env => env
                            .ConfigureDeserializer(d => d
                                .Add(ConverterFactory
                                    .Create<JToken, JToken>((token, env, _, recurse) => token)))))
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
            Formatting.Indented));

        protected override SettingsTask ModifySettingsTask(SettingsTask task) => base
            .ModifySettingsTask(task)
            .DontScrubGuids()
            .ScrubGuids();
    }
}
