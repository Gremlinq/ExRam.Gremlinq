using System.Collections.Generic;
using System.Linq;
using ExRam.Gremlinq.Core;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Providers.Tests
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

            public IAsyncEnumerable<object> Execute(object groovySerializedQuery)
            {
                return AsyncEnumerableEx
                    .Return(JToken.Parse(_json));
            }
        }

        public static IGremlinQuerySource WithExecutor(this IGremlinQuerySource source, string json)
        {
            return source.ConfigureEnvironment(env => env
                .UseSerializer(GremlinQuerySerializer.Default)
                .UseExecutor(new TestJsonQueryExecutor(json))
                .UseDeserializer(GremlinQueryExecutionResultDeserializer.Graphson));
        }
    }
}
