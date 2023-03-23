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
        public abstract class Fixture : GremlinqTestFixture
        {
            protected Fixture(string sourcePrefix, IGremlinQuerySource source) : base(source
                .ConfigureEnvironment(env => env
                    .UseExecutor(GremlinQueryExecutor.Create((_, _) =>
                    {
                        var context = XunitContext.Context;

                        try
                        {
                            var jArray = JsonConvert.DeserializeObject<JArray>(
                                File.ReadAllText(System.IO.Path.Combine(context.SourceDirectory, sourcePrefix + "." + context.MethodName + ".verified.txt")));

                            return jArray?
                                .Where(obj => !(obj is JObject jObject && jObject.ContainsKey("serverException")))
                                .Cast<object>()
                                .ToAsyncEnumerable() ?? AsyncEnumerable.Empty<object>();
                        }
                        catch (IOException)
                        {
                            return AsyncEnumerable.Empty<object>();
                        }
                    }))))
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
