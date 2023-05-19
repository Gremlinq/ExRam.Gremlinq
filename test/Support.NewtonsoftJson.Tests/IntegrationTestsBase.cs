using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core.Tests;

namespace ExRam.Gremlinq.Support.NewtonsoftJson.Tests
{
    public abstract class IntegrationTestsBase : QueryExecutionTest
    {
        protected IntegrationTestsBase(IntegrationTestFixture fixture, ITestOutputHelper testOutputHelper, [CallerFilePath] string callerFilePath = "") : base(
            fixture,
            testOutputHelper,
            callerFilePath)
        {
        }

    }
}
