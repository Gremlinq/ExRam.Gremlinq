using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Tests.Infrastructure;
using Gremlin.Net.Structure;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class Issue37 : GremlinqTestBase
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

        public Issue37() : base(new DebugGremlinQueryVerifier())
        {

        }

        [Fact]
        public Task Working() => g
            .ConfigureEnvironment(env => env
                .UseModel(GraphModel.FromBaseTypes<VertexBase, Edge>()))
            .AddV(new Item { Value = "MyValue" })
            .Verify();

        [Fact]
        public Task Buggy() => g
            .ConfigureEnvironment(env => env
                .UseModel(GraphModel.FromBaseTypes<VertexBase, Edge>()))
            .AddV(new ItemOverride { Value = "MyValue" })
            .Verify();
    }
}
