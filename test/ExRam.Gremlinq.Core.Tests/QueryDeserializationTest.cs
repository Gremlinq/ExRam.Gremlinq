using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class QueryDeserializationTest : QueryExecutionTest
    {
        public abstract class Fixture : GremlinqTestFixture
        {
            protected Fixture(IGremlinQuerySource source) : base(source
                .ConfigureEnvironment(env => env
                    .UseExecutor(GremlinQueryExecutor.Create((_, _) =>
                    {
                        var context = XunitContext.Context;

                        var prefix = context.ClassName.Substring(0, context.ClassName.Length - "DeserializationTests".Length);

                        try
                        {
                            var jArray = JsonConvert.DeserializeObject<JArray>(
                                File.ReadAllText(System.IO.Path.Combine(context.SourceDirectory, prefix + "IntegrationTests." + XunitContext.Context.MethodName + ".verified.txt")));

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
