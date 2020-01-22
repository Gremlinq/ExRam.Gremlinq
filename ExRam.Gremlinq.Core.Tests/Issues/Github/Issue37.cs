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
            public object? Id { get; set; }
        }

        public class Item : VertexBase
        {
            public string Value { get; set; }
        }

        public abstract class VertexBaseAbstract : IVertex
        {
            public abstract string PartitionKey { get; set; }
            public object? Id { get; set; }
        }

        public class ItemOverride : VertexBaseAbstract
        {
            public string Value { get; set; }

            public override string PartitionKey { get; set; } = "MyKey";
        }

        [Fact]
        public async Task Working()
        {
            var _g = g.ConfigureEnvironment(env => env
                .UseExecutionPipeline(GremlinQueryExecutionPipeline.EchoGroovy));

            _g
                .AddV(new Item {Value = "MyValue"})
                .Should()
                .SerializeToGroovy("addV(_a).property(single, _b, _c).property(single, _d, _e).project(_f, _g, _h, _i).by(id).by(label).by(__.constant(_j)).by(__.properties().group().by(__.label()).by(__.project(_f, _g, _k, _i).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Item", "PartitionKey", "MyKey", "Value", "MyValue", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public async Task Buggy()
        {
            var _g = g.ConfigureEnvironment(env => env
                .UseExecutionPipeline(GremlinQueryExecutionPipeline.EchoGroovy));

            _g
                .AddV(new ItemOverride { Value = "MyValue" })
                .Should()
                .SerializeToGroovy("addV(_a).property(single, _b, _c).property(single, _d, _e).project(_f, _g, _h, _i).by(id).by(label).by(__.constant(_j)).by(__.properties().group().by(__.label()).by(__.project(_f, _g, _k, _i).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("ItemOverride", "PartitionKey", "MyKey", "Value", "MyValue", "id", "label", "type", "properties", "vertex", "value");
        }
    }
}
