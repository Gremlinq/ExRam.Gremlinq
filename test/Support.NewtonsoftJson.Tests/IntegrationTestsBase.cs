using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Tests.Entities;

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

        [Fact]
        public virtual async Task AddV_list_cardinality_id()
        {
            await _g
               .ConfigureEnvironment(env => env
                   .UseModel(GraphModel
                       .FromBaseTypes<VertexWithListId, Edge>(lookup => lookup
                           .IncludeAssembliesOfBaseTypes())))
               .AddV(new VertexWithListId { Id = new[] { "123", "456" } })
               .Verify();
        }
    }
}
