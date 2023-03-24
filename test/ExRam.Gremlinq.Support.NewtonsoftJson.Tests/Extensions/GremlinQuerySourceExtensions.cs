using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Serialization;
using Gremlin.Net.Process.Traversal;

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

            public IAsyncEnumerable<object> Execute(Bytecode bytecode, IGremlinQueryEnvironment environment)
            {
                var token = JsonConvert.DeserializeObject<JToken>(
                    _json,
                    new JsonSerializerSettings()
                    {
                        DateParseHandling = DateParseHandling.None
                    })!;

                if (token is JArray jArray)
                    return jArray.Cast<object>().ToAsyncEnumerable();

                return new object[]
                {
                    token   
                }.ToAsyncEnumerable();
            }
        }

        public static IGremlinQuerySource WithExecutor(this IGremlinQuerySource source, string json)
        {
            return source.ConfigureEnvironment(env => env
                .UseSerializer(Serializer.Default)
                .UseNewtonsoftJson()
                .UseExecutor(new TestJsonQueryExecutor(json)));
        }
    }
}
