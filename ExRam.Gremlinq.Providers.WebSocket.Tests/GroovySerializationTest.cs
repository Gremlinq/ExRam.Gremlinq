using System.Linq;
using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Tests.Entities;
using Xunit;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.WebSocket.Tests
{
    public class GroovySerializationTest : GroovySerializationTest<GroovyGremlinQueryElementVisitor>
    {
        [Fact]
        public void Where_empty_array_intersects_property_array()
        {
            g
                .V<Company>()
                .Where(t => new string[0].Intersect(t.PhoneNumbers).Any())
                .Should()
                .SerializeToGroovy<GroovyGremlinQueryElementVisitor>("g.V().hasLabel(_a).none()")
                .WithParameters("Company");
        }

        [Fact]
        public void Where_property_array_intersects_empty_array()
        {
            g
                .V<Company>()
                .Where(t => t.PhoneNumbers.Intersect(new string[0]).Any())
                .Should()
                .SerializeToGroovy<GroovyGremlinQueryElementVisitor>("g.V().hasLabel(_a).none()")
                .WithParameters("Company");
        }


        [Fact]
        public void Where_property_is_contained_in_empty_enumerable()
        {
            var enumerable = Enumerable.Empty<int>();

            g
                .V<Person>()
                .Where(t => enumerable.Contains(t.Age))
                .Should()
                .SerializeToGroovy<GroovyGremlinQueryElementVisitor>("g.V().hasLabel(_a).none()")
                .WithParameters("Person");
        }

        [Fact]
        public void OutE_of_no_derived_types()
        {
            g
                .V()
                .OutE<string>()
                .Should()
                .SerializeToGroovy<GroovyGremlinQueryElementVisitor>("g.V().none()")
                .WithoutParameters();
        }
    }
}
