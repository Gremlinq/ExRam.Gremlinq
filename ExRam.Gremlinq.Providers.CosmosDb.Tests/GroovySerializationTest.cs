using System.Linq;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Tests.Entities;
using Xunit;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public class CosmosDbGroovySerializationTest : GroovySerializationTest
    {
        public CosmosDbGroovySerializationTest() : base(g.UseCosmosDb("localhost", "database", "graph", "authKey"))
        {

        }

        [Fact]
        public void Where_property_array_intersects_empty_array()
        {
            _g
                .V<Company>()
                .Where(t => t.PhoneNumbers.Intersect(new string[0]).Any())
                .Should()
                .SerializeToGroovy("g.V().hasLabel(_a).not(__.identity())")
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
                .SerializeToGroovy("g.V().hasLabel(_a).not(__.identity())")
                .WithParameters("Person");
        }

        [Fact]
        public void OutE_of_no_derived_types()
        {
            _g
                .V()
                .OutE<string>()
                .Should()
                .SerializeToGroovy("g.V().not(__.identity())")
                .WithoutParameters();
        }

        [Fact]
        public void Skip_translates_to_range()
        {
            _g
                .V()
                .Skip(10)
                .Should()
                .SerializeToGroovy("g.V().range(_a, _b)")
                .WithParameters(10, -1);
        }

        [Fact]
        public void Where_none_traversal()
        {
            _g
                .V<Person>()
                .Where(_ => _.None())
                .Should()
                .SerializeToGroovy("g.V().hasLabel(_a).not(__.identity())")
                .WithParameters("Person");
        }

        [Fact]
        public void And_none()
        {
            _g
                .V<Person>()
                .And(
                    __ => __.None())
                .Should()
                .SerializeToGroovy("g.V().hasLabel(_a).not(__.identity())")
                .WithParameters("Person");
        }
    }
}
