using System;
using System.Linq;
using System.Threading.Tasks;
using ExRam.Gremlinq.Tests.Entities;
using FluentAssertions;
using Xunit;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class ExecutionPipelinesTest
    {
        private interface IFancyId
        {
            string Id { get; set; }
        }

        private class FancyId : IFancyId
        {
            public string Id { get; set; }
        }

        private class EvenMoreFancyId : FancyId
        {
        }

        [Fact]
        public async Task Echo()
        {
            var query = await g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<Vertex, Edge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes()))
                    .UseExecutionPipeline(GremlinQueryExecutionPipeline.EchoGroovy))
                .V<Person>()
                .Where(x => x.Age == 36)
                .Cast<string>()
                .FirstAsync();

            query
                .Should()
                .Be("V().hasLabel(_a).has(_b, _c).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))");
        }

        [Fact]
        public void Echo_wrong_type()
        {
            GremlinQuerySource.g
                .ConfigureEnvironment(env => env
                    .UseExecutionPipeline(GremlinQueryExecutionPipeline.EchoGraphson))
                .V<Person>()
                .Awaiting(async _ => await _
                    .ToArrayAsync())
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Fact]
        public void OverrideAtomSerializer()
        {
            g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<Vertex, Edge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes()))
                    .ConfigureExecutionPipeline(_ => GremlinQueryExecutionPipeline
                        .EchoGroovy
                        .ConfigureSerializer(_ => _
                            .OverrideFragmentSerializer<FancyId>((key, overridden, recurse) => recurse(key.Id)))))
                .V<Person>(new FancyId {Id = "someId"})
                .Should()
                .SerializeToGroovy("V(_a).hasLabel(_b).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("someId", "Person", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void OverrideAtomSerializer_recognizes_derived_type()
        {
            g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<Vertex, Edge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes()))
                    .ConfigureExecutionPipeline(_ => GremlinQueryExecutionPipeline
                        .EchoGroovy
                        .ConfigureSerializer(_ => _
                            .OverrideFragmentSerializer<FancyId>((key, overridden, recurse) => recurse(key.Id)))))
                .V<Person>(new EvenMoreFancyId { Id = "someId" })
                .Should()
                .SerializeToGroovy("V(_a).hasLabel(_b).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("someId", "Person", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void OverrideAtomSerializer_recognizes_interface()
        {
            g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<Vertex, Edge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes()))
                    .ConfigureExecutionPipeline(_ => GremlinQueryExecutionPipeline
                        .EchoGroovy
                        .ConfigureSerializer(_ => _
                            .OverrideFragmentSerializer<IFancyId>((key, overridden, recurse) => recurse(key.Id)))))
                .V<Person>(new FancyId { Id = "someId" })
                .Should()
                .SerializeToGroovy("V(_a).hasLabel(_b).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("someId", "Person", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void OverrideAtomSerializer_recognizes_interface_through_derived_type()
        {
            g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<Vertex, Edge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes()))
                    .ConfigureExecutionPipeline(_ => GremlinQueryExecutionPipeline
                        .EchoGroovy
                        .ConfigureSerializer(_ => _
                            .OverrideFragmentSerializer<IFancyId>((key, overridden, recurse) => recurse(key.Id)))))
                .V<Person>(new FancyId { Id = "someId" })
                .Should()
                .SerializeToGroovy("V(_a).hasLabel(_b).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("someId", "Person", "id", "label", "type", "properties", "vertex", "value");
        }
    }
}
