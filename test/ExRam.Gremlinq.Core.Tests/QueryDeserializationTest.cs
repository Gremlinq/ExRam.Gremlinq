using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core.Execution;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class QueryDeserializationTest : QueryExecutionTest
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
                                File.ReadAllText(System.IO.Path.Combine(context.SourceDirectory, sourcePrefix + "." + XunitContext.Context.MethodName + ".verified.txt")));

                            return jArray?
                                .Select(x => (object)x)
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

        protected QueryDeserializationTest(Fixture fixture, ITestOutputHelper testOutputHelper, [CallerFilePath] string callerFilePath = "") : base(
            fixture,
            testOutputHelper,
            callerFilePath)
        {
        }
    }
}
