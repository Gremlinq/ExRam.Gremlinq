using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Tests;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Support.NewtonsoftJson.Tests
{
    public abstract class DeserializationTestsBase : QueryExecutionTest
    {
        private sealed class DeserializationGremlinQueryExecutor : IGremlinQueryExecutor
        {
            private readonly string _sourcePrefix;

            public DeserializationGremlinQueryExecutor(string sourcePrefix)
            {
                _sourcePrefix = sourcePrefix;
            }

            public IAsyncEnumerable<object> Execute(IGremlinQueryBase query, IGremlinQueryEnvironment environment)
            {
                var context = XunitContext.Context;

                try
                {
                    var jArray = JsonConvert.DeserializeObject<JArray>(
                        File.ReadAllText(System.IO.Path.Combine(context.SourceDirectory, _sourcePrefix + "." + context.MethodName + ".verified.txt")));

                    return jArray?
                        .Where(obj => !(obj is JObject jObject && jObject.ContainsKey("ServerException")))
                        .Cast<object>()
                        .ToAsyncEnumerable() ?? AsyncEnumerable.Empty<object>();
                }
                catch (IOException)
                {
                    return AsyncEnumerable.Empty<object>();
                }
            }
        }

        public abstract class Fixture : GremlinqTestFixture
        {
            protected Fixture(string sourcePrefix, IGremlinQuerySource source) : base(source
                .ConfigureEnvironment(env => env
                    .UseExecutor(new DeserializationGremlinQueryExecutor(sourcePrefix))))
            {
            }
        }

        protected DeserializationTestsBase(Fixture fixture, ITestOutputHelper testOutputHelper, [CallerFilePath] string callerFilePath = "") : base(
            fixture,
            testOutputHelper,
            callerFilePath)
        {
        }
    }
}
