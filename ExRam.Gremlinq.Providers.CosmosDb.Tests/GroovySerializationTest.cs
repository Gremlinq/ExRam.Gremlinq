using System;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Tests.Entities;
using Xunit;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public class CosmosDbGroovySerializationTest : GroovySerializationTest
    {
        public CosmosDbGroovySerializationTest() : base(g.UseCosmosDb(new Uri("wss://localhost"), "database", "graph", "authKey"))
        {

        }
        
        [Fact]
        public void Skip_translates_to_range()
        {
            _g
                .V()
                .Skip(10)
                .Should()
                .SerializeToGroovy("V().range(_a, _b).project('id', 'label', 'type', 'properties').by(id).by(label).by(__.constant('vertex')).by(__.properties().group().by(__.label()).by(__.project('id', 'label', 'value', 'properties').by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters(10, -1);
        }

        [Fact]
        public void StringKey()
        {
            _g
                .V<Person>("id")
                .Should()
                .SerializeToGroovy("V(_a).hasLabel(_b).project('id', 'label', 'type', 'properties').by(id).by(label).by(__.constant('vertex')).by(__.properties().group().by(__.label()).by(__.project('id', 'label', 'value', 'properties').by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("id", "Person");
        }

        [Fact]
        public void CosmosDbKey()
        {
            _g
                .V<Person>(new CosmosDbKey("pk", "id"))
                .Should()
                .SerializeToGroovy("V(_a).hasLabel(_b).project('id', 'label', 'type', 'properties').by(id).by(label).by(__.constant('vertex')).by(__.properties().group().by(__.label()).by(__.project('id', 'label', 'value', 'properties').by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters(new[] { "pk", "id" }, "Person");
        }

        [Fact]
        public void CosmosDbKey_with_null_partitionKey()
        {
            _g
                .V<Person>(new CosmosDbKey("id"))
                .Should()
                .SerializeToGroovy("V(_a).hasLabel(_b).project('id', 'label', 'type', 'properties').by(id).by(label).by(__.constant('vertex')).by(__.properties().group().by(__.label()).by(__.project('id', 'label', 'value', 'properties').by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("id", "Person");
        }

        [Fact]
        public void Mixed_StringKey_CosmosDbKey()
        {
            _g
                .V<Person>(new CosmosDbKey("pk", "id"), "id2")
                .Should()
                .SerializeToGroovy("V(_a, _b).hasLabel(_c).project('id', 'label', 'type', 'properties').by(id).by(label).by(__.constant('vertex')).by(__.properties().group().by(__.label()).by(__.project('id', 'label', 'value', 'properties').by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters(new[] { "pk", "id" }, "id2", "Person");
        }

        [Fact]
        public void Mixed_StringKey_CosmosDbKey_with_null_partitionKey()
        {
            _g
                .V<Person>(new CosmosDbKey("id"), "id2")
                .Should()
                .SerializeToGroovy("V(_a, _b).hasLabel(_c).project('id', 'label', 'type', 'properties').by(id).by(label).by(__.constant('vertex')).by(__.properties().group().by(__.label()).by(__.project('id', 'label', 'value', 'properties').by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("id", "id2", "Person");
        }
    }
}
