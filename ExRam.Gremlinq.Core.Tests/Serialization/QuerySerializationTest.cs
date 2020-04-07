using System.Runtime.CompilerServices;
using Xunit.Abstractions;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class QuerySerializationTest : QueryExecutionTest
    {
        protected QuerySerializationTest(IGremlinQuerySource g, ITestOutputHelper testOutputHelper, [CallerFilePath] string callerFilePath = "") : base(
            g
                .ConfigureEnvironment(env => env
                    .UseExecutor(GremlinQueryExecutor.Echo)
                    .UseDeserializer(GremlinQueryExecutionResultDeserializer.Identity)),
            testOutputHelper,
            callerFilePath)
        {
        }
    }
}
