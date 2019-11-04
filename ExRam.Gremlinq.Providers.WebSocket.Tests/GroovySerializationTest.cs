using System.Linq;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Tests.Entities;
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
        public void Where_empty_array_intersects_property_array()
        {
            _g
                .V<Company>()
                .Where(t => new string[0].Intersect(t.PhoneNumbers).Any())
                .Should()
                .SerializeToGroovy("g.V().hasLabel(_a).none()")
                .WithParameters("Company");
        }

        [Fact]
        public void Where_property_array_intersects_empty_array()
        {
            _g
                .V<Company>()
                .Where(t => t.PhoneNumbers.Intersect(new string[0]).Any())
                .Should()
                .SerializeToGroovy("g.V().hasLabel(_a).none()")
                .WithParameters("Company");
        }


        [Fact]
        public void Where_property_is_contained_in_empty_enumerable()
        {
            var enumerable = Enumerable.Empty<int>();

            _g
                .V<Person>()
                .Where(t => enumerable.Contains(t.Age))
                .Should()
                .SerializeToGroovy("g.V().hasLabel(_a).none()")
                .WithParameters("Person");
        }

        [Fact]
        public void OutE_of_no_derived_types()
        {
            _g
                .V()
                .OutE<string>()
                .Should()
                .SerializeToGroovy("g.V().none()")
                .WithoutParameters();
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

        [Fact]
        public void Where_none_traversal()
        {
            _g
                .V<Person>()
                .Where(_ => _.None())
                .Should()
                .SerializeToGroovy("g.V().hasLabel(_a).none()")
                .WithParameters("Person");
        }
    }
}
