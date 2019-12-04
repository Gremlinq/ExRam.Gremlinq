using System.Threading.Tasks;
using ExRam.Gremlinq.Core.GraphElements;
using Xunit;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class Issue37
    {
        public class VertexBase : IVertex
        {
            public string PartitionKey { get; set; } = "MyKey";
            public object Id { get; set; }
        }

        public class Item : VertexBase
        {
            public string Value { get; set; }
        }

        public abstract class VertexBaseAbstract : IVertex
        {
            public abstract string PartitionKey { get; set; }
            public object Id { get; set; }
        }

        public class ItemOverride : VertexBaseAbstract
        {
            public string Value { get; set; }

            public override string PartitionKey { get; set; } = "MyKey";
        }

        [Fact]
        public async Task Working()
        {
            var _g = g.UseExecutionPipeline(GremlinQueryExecutionPipeline.EchoGroovy);

            _g
                .AddV(new Item {Value = "MyValue"})
                .Should()
                .SerializeToGroovy("addV(_a).property(single, _b, _c).property(single, _d, _e)")
                .WithParameters("Item", "PartitionKey", "MyKey", "Value", "MyValue");
        }

        [Fact]
        public async Task Buggy()
        {
            var _g = g.UseExecutionPipeline(GremlinQueryExecutionPipeline.EchoGroovy);

            _g
                .AddV(new ItemOverride { Value = "MyValue" })
                .Should()
                .SerializeToGroovy("addV(_a).property(single, _b, _c).property(single, _d, _e)")
                .WithParameters("ItemOverride", "PartitionKey", "MyKey", "Value", "MyValue");
        }
    }
}
