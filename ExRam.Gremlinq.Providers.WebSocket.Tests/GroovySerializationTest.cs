using ExRam.Gremlinq.Core.Tests;
using Xunit;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.WebSocket.Tests
{
    public class DefaultGroovySerializationTest : GroovySerializationTest
    {
        public DefaultGroovySerializationTest() : base(g.UseWebSocket("localhost", GraphsonVersion.V2))
        {

        }
        
        [Fact]
        public void Skip_remains_skip()
        {
            _g
                .V()
                .Skip(10)
                .Should()
                .SerializeToGroovy("g.V().skip(_a)")
                .WithParameters(10);
        }
    }
}
