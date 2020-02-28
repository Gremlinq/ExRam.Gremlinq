using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.WebSocket;
using ExRam.Gremlinq.Tests.Entities;
using Xunit;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public class GremlinServerGroovySerializationTest : GroovySerializationTest
    {
        public GremlinServerGroovySerializationTest() : base(g
            .ConfigureEnvironment(env => env
                .UseGremlinServer(builder => builder
                    .AtLocalhost()
                    .SetGraphSONVersion(GraphsonVersion.V2))))
        {

        }

        [Fact]
        public void AddV_with_enum_property_with_workaround()
        {
            _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(options => options
                        .SetItem(GremlinServerGremlinqOptions.WorkaroundTinkerpop2112, true)))
                .AddV(new Person { Id = 1, Gender = Gender.Female })
                .Should()
                .SerializeToGroovy("addV(_a).property(id, _b).property(single, _c, _d).property(single, _e, _f).project(_g, _h, _i, _j).by(id).by(label).by(__.constant(_k)).by(__.properties().group().by(__.label()).by(__.project(_g, _h, _l, _j).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", 1, "Gender", 1, "Age", 0, "id", "label", "type", "properties", "vertex", "value");
        }
    }
}
