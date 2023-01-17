using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Core.Deserialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core.Tests
{
    public static class GremlinQuerySourceExtensions
    {
        private sealed class TestJsonQueryExecutor : IGremlinQueryExecutor
        {
            private readonly string _json;

            public TestJsonQueryExecutor(string json)
            {
                _json = json;
            }

            public IAsyncEnumerable<object> Execute(ISerializedGremlinQuery serializedQuery, IGremlinQueryEnvironment environment)
            {
                return new object[]
                {
                    JsonConvert.DeserializeObject<JToken>(
                        _json,
                        new JsonSerializerSettings()
                        {
                            DateParseHandling = DateParseHandling.None
                        })!
                }.ToAsyncEnumerable();
            }
        }

        public static IGremlinQuerySource WithExecutor(this IGremlinQuerySource source, string json)
        {
            return source.ConfigureEnvironment(env => env
                .UseSerializer(GremlinQuerySerializer.Default)
                .UseExecutor(new TestJsonQueryExecutor(json))
                .ConfigureDeserializer(d => d
                    .ConfigureFragmentDeserializer(f => f
                        .AddNewtonsoftJson())));
        }

        public static IGremlinQuerySource UseDebuggingExecutor(this IConfigurableGremlinQuerySource source)
        {
            return source
                .ConfigureEnvironment(env => env
                    .UseExecutor(GremlinQueryExecutor.Create((query, env) =>
                    {
                        return AsyncEnumerable.Create(Core);

                        async IAsyncEnumerator<object> Core(CancellationToken ct)
                        {
                            yield return env.Debugger.TryToString(query, env)!;
                        }
                    })));
        }
    }
}
