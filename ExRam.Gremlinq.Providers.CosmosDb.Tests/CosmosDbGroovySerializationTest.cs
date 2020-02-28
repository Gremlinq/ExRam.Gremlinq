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
        public CosmosDbGroovySerializationTest() : base(g.ConfigureEnvironment(env => env
            .UseCosmosDb(builder => builder
                .At(new Uri("wss://localhost"), "database", "graph")
                .AuthenticateBy("authKey"))))
        {

        }
        
        [Fact]
        public void Skip_translates_to_range()
        {
            _g
                .V()
                .Skip(10)
                .Should()
                .SerializeToGroovy("V().range(_a, _b).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters(10, -1, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void StringKey()
        {
            _g
                .V<Person>("id")
                .Should()
                .SerializeToGroovy("V(_a).hasLabel(_b).project(_a, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_a, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("id", "Person", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void CosmosDbKey()
        {
            _g
                .V<Person>(new CosmosDbKey("pk", "id"))
                .Should()
                .SerializeToGroovy("V(_a).hasLabel(_b).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters(new[] { "pk", "id" }, "Person", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void CosmosDbKey_with_null_partitionKey()
        {
            _g
                .V<Person>(new CosmosDbKey("id"))
                .Should()
                .SerializeToGroovy("V(_a).hasLabel(_b).project(_a, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_a, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("id", "Person", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Mixed_StringKey_CosmosDbKey()
        {
            _g
                .V<Person>(new CosmosDbKey("pk", "id"), "id2")
                .Should()
                .SerializeToGroovy("V(_a, _b).hasLabel(_c).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters(new[] { "pk", "id" }, "id2", "Person", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Mixed_StringKey_CosmosDbKey_with_null_partitionKey()
        {
            _g
                .V<Person>(new CosmosDbKey("id"), "id2")
                .Should()
                .SerializeToGroovy("V(_a, _b).hasLabel(_c).project(_a, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_a, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("id", "id2", "Person", "label", "type", "properties", "vertex", "value");
        }
    }
}
