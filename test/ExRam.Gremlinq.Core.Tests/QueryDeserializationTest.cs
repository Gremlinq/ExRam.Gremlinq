using System.Runtime.CompilerServices;

using Xunit.Abstractions;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class QueryDeserializationTest : QueryExecutionTest
    {
        protected QueryDeserializationTest(IGremlinQuerySource g, ITestOutputHelper testOutputHelper, [CallerFilePath] string callerFilePath = "") : base(g, testOutputHelper, callerFilePath)
        {
        }
    }
}
