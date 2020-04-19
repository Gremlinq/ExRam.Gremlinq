using System.Threading.Tasks;
using ExRam.Gremlinq.Core.GraphElements;
using NullGuard;
using Xunit;
using Xunit.Abstractions;
using VerifyXunit;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class Issue37 : VerifyBase
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

        public Issue37(ITestOutputHelper output) : base(output)
        {

        }

        [Fact]
        public async Task Working()
        {
            var _g = g.ConfigureEnvironment(env => env
                .EchoGroovy());

            await _g
                .AddV(new Item { Value = "MyValue" })
                .Verify(this);
        }

        [Fact]
        public async Task Buggy()
        {
            var _g = g.ConfigureEnvironment(env => env
                .EchoGroovy());

            await _g
                .AddV(new ItemOverride { Value = "MyValue" })
                .Verify(this);
        }
    }
}
