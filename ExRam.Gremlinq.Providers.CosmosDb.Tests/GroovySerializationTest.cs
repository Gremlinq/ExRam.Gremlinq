using System;
using System.Linq;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Tests.Entities;
using FluentAssertions;
using Xunit;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public class GroovySerializationTest : GroovySerializationTest<GremlinQuerySourceExtensions.CosmosDbGroovyGremlinQueryElementVisitor>
    {
        [Fact]
        public void Limit_overflow()
        {
            g
                .V()
                .Limit((long)int.MaxValue + 1)
                .Invoking(x => new GremlinQuerySourceExtensions.CosmosDbGroovyGremlinQueryElementVisitor().Visit(x))
                .Should()
                .Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Where_property_array_intersects_empty_array()
        {
            g
                .V<Company>()
                .Where(t => t.PhoneNumbers.Intersect(new string[0]).Any())
                .Should()
                .SerializeToGroovy<GremlinQuerySourceExtensions.CosmosDbGroovyGremlinQueryElementVisitor>("g.V().hasLabel(_a).not(__.identity())")
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
                .SerializeToGroovy<GremlinQuerySourceExtensions.CosmosDbGroovyGremlinQueryElementVisitor>("g.V().hasLabel(_a).not(__.identity())")
                .WithParameters("Person");
        }

        [Fact]
        public void OutE_of_no_derived_types()
        {
            g
                .V()
                .OutE<string>()
                .Should()
                .SerializeToGroovy<GremlinQuerySourceExtensions.CosmosDbGroovyGremlinQueryElementVisitor>("g.V().not(__.identity())")
                .WithoutParameters();
        }
    }
}
