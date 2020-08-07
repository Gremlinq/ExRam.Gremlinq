using System.Runtime.CompilerServices;
using Xunit.Abstractions;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class QueryIntegrationTest : QueryExecutionTest
    {
        protected QueryIntegrationTest(IGremlinQuerySource g, ITestOutputHelper testOutputHelper, [CallerFilePath] string callerFilePath = "") : base(g, testOutputHelper, callerFilePath)
        {
        }
    }
}
