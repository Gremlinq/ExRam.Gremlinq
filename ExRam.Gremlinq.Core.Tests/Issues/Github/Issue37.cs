using System.Threading.Tasks;
using ExRam.Gremlinq.Core.GraphElements;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    [UsesVerify]
    public class Issue37 : XunitContextBase
    {
        public class VertexBase : IVertex
        {
            public string PartitionKey { get; set; } = "MyKey";
            public object? Id { get; set; }
        }

        public class Item : VertexBase
        {
            public string? Value { get; set; }
        }

        public abstract class VertexBaseAbstract : IVertex
        {
            public abstract string PartitionKey { get; set; }
            public object? Id { get; set; }
        }

        public class ItemOverride : VertexBaseAbstract
        {
            public string? Value { get; set; }

            public override string PartitionKey { get; set; } = "MyKey";
        }

        public Issue37(ITestOutputHelper output) : base(output)
        {

        }

        [Fact]
        public async Task Working()
        {
            await g
                .ConfigureEnvironment(env => env
                    .EchoGroovyString())
                .AddV(new Item { Value = "MyValue" })
                .Cast<string>()
                .Verify(this);
        }

        [Fact]
        public async Task Buggy()
        {
            await g
                .ConfigureEnvironment(env => env
                    .EchoGroovyString())
                .AddV(new ItemOverride { Value = "MyValue" })
                .Cast<string>()
                .Verify(this);
        }
    }
}
