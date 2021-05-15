using System.Collections.Generic;
using System.Linq;
using ExRam.Gremlinq.Core.Deserialization;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Serialization;
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

            public IAsyncEnumerable<object> Execute(object groovySerializedQuery, IGremlinQueryEnvironment environment)
            {
                return new [] { JToken.Parse(_json) }.ToAsyncEnumerable();
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
    }
}
