using System;
using System.Linq;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.WebSocket;
using ExRam.Gremlinq.Tests.Entities;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public class GremlinServerGroovySerializationTest : GroovySerializationTest
    {
        public GremlinServerGroovySerializationTest(ITestOutputHelper testOutputHelper) : base(
            g
                .ConfigureEnvironment(env => env
                .UseGremlinServer(builder => builder
                    .AtLocalhost()
                    .SetGraphSONVersion(GraphsonVersion.V2))),
            testOutputHelper)
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

        [Fact]
        public void StepLabel_of_array_contains_element_graphson()
        {
            _g
                .Inject(1, 2, 3)
                .Fold()
                .As((_, ints) => _
                    .V<Person>()
                    .Where(person => ints.Value.Contains(person.Age)))
                .Should()
                .SerializeToGraphson("{\"@type\":\"g:Bytecode\",\"@value\":{\"step\":[[\"inject\",{\"@type\":\"g:Int32\",\"@value\":1},{\"@type\":\"g:Int32\",\"@value\":2},{\"@type\":\"g:Int32\",\"@value\":3}],[\"fold\"],[\"as\",\"l1\"],[\"V\"],[\"hasLabel\",\"Person\"],[\"has\",\"Age\",{\"@type\":\"g:Bytecode\",\"@value\":{\"step\":[[\"where\",{\"@type\":\"g:P\",\"@value\":{\"predicate\":\"within\",\"value\":[\"l1\"]}}]]}}],[\"project\",\"id\",\"label\",\"type\",\"properties\"],[\"by\",{\"@type\":\"g:T\",\"@value\":\"id\"}],[\"by\",{\"@type\":\"g:T\",\"@value\":\"label\"}],[\"by\",{\"@type\":\"g:Bytecode\",\"@value\":{\"step\":[[\"constant\",\"vertex\"]]}}],[\"by\",{\"@type\":\"g:Bytecode\",\"@value\":{\"step\":[[\"properties\"],[\"group\"],[\"by\",{\"@type\":\"g:Bytecode\",\"@value\":{\"step\":[[\"label\"]]}}],[\"by\",{\"@type\":\"g:Bytecode\",\"@value\":{\"step\":[[\"project\",\"id\",\"label\",\"value\",\"properties\"],[\"by\",{\"@type\":\"g:T\",\"@value\":\"id\"}],[\"by\",{\"@type\":\"g:Bytecode\",\"@value\":{\"step\":[[\"label\"]]}}],[\"by\",{\"@type\":\"g:Bytecode\",\"@value\":{\"step\":[[\"value\"]]}}],[\"by\",{\"@type\":\"g:Bytecode\",\"@value\":{\"step\":[[\"valueMap\"]]}}],[\"fold\"]]}}]]}}]]}}");
        }


        [Fact]
        public void Skip_underflow()
        {
            _g
                .V()
                .Invoking(_ => _.Skip(-1))
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public void SkipGlobal()
        {
            _g
                .V()
                .Skip(1)
                .Should()
                .SerializeToGroovy("V().skip(_a).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters(1, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void SkipLocal()
        {
            _g
                .V()
                .SkipLocal(1)
                .Should()
                .SerializeToGroovy("V().skip(local, _a).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters(1, "id", "label", "type", "properties", "vertex", "value");
        }
    }
}
