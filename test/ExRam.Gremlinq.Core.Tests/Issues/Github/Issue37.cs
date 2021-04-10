using System.Threading.Tasks;
using ExRam.Gremlinq.Core.GraphElements;

using Gremlin.Net.Structure;

using Xunit;
using Xunit.Abstractions;

using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class Issue37 : GremlinqTestBase
    {
        public class VertexBase
        {
            public string PartitionKey { get; set; } = "MyKey";
            public object? Id { get; set; }
        }

        public class Item : VertexBase
        {
            public string? Value { get; set; }
        }

        public abstract class VertexBaseAbstract
        {
            public abstract string PartitionKey { get; set; }
            public object? Id { get; set; }
        }

        public class ItemOverride : VertexBaseAbstract
        {
            public string? Value { get; set; }

            public override string PartitionKey { get; set; } = "MyKey";
        }

        public Issue37(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public async Task Working()
        {
            await g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel.FromBaseTypes<VertexBase, Edge>())
                    .EchoGroovyString())
                .AddV(new Item { Value = "MyValue" })
                .Cast<string>()
                .Verify();
        }

        [Fact]
        public async Task Buggy()
        {
            await g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel.FromBaseTypes<VertexBase, Edge>())
                    .EchoGroovyString())
                .AddV(new ItemOverride { Value = "MyValue" })
                .Cast<string>()
                .Verify();
        }
    }
}
