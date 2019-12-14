using System;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using Xunit;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.WebSocket.Tests
{
    public class DefaultGroovySerializationTest : GroovySerializationTest
    {
        public DefaultGroovySerializationTest() : base(g
            .UseWebSocket(new Uri("ws://localhost"), GraphsonVersion.V2)
            .ConfigureEnvironment(env => env
                .ConfigureExecutionPipeline(p => p
                    .ConfigureSerializer(s => s
                        .ToGroovy()))))
        {

        }
        
        [Fact]
        public void Skip_remains_skip()
        {
            _g
                .V()
                .Skip(10)
                .Should()
                .SerializeToGroovy("V().skip(_a).project('id', 'label', 'type', 'properties').by(id).by(label).by(__.constant('vertex')).by(__.properties().group().by(__.label()).by(__.project('id', 'label', 'value', 'properties').by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters(10);
        }
    }
}
