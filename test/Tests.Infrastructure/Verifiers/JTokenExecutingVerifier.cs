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

        public JTokenExecutingVerifier(Func<SettingsTask, SettingsTask>? settingsTaskModifier = null, [CallerFilePath] string sourceFile = "") : base(settingsTaskModifier, sourceFile)
        {
        }

        public override SettingsTask Verify<TElement>(IGremlinQueryBase<TElement> query) => InnerVerify(Execute(query));

        private async Task<string> Execute<TElement>(IGremlinQueryBase<TElement> query) => JsonConvert.SerializeObject(
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
            Formatting.Indented);

        protected override SettingsTask InnerVerify<T>(ValueTask<T> value) => base
            .InnerVerify(value)
            .ScrubGuids();
    }
}
