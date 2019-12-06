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
                .UseModel(GraphModel
                    .FromBaseTypes<Vertex, Edge>())
                .UseExecutionPipeline(GremlinQueryExecutionPipeline.EchoGroovy)
                .V<Person>()
                .Where(x => x.Age == 36)
                .Cast<string>()
                .FirstAsync();

            query
                .Should()
                .Be("V().hasLabel(_a).has(_b, _c)");
        }

        [Fact]
        public void Echo_wrong_type()
        {
            GremlinQuerySource.g
                .UseExecutionPipeline(GremlinQueryExecutionPipeline.EchoGraphson)
                .V()
                .Awaiting(async _ => await _
                    .ToArrayAsync())
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Fact]
        public void OverrideAtomSerializer()
        {
            g
                .UseModel(GraphModel
                    .FromBaseTypes<Vertex, Edge>())
                .ConfigureExecutionPipeline(_ => GremlinQueryExecutionPipeline
                    .EchoGroovy
                    .ConfigureSerializer(_ => _
                        .OverrideFragmentSerializer<FancyId>((key, overridden, recurse) => recurse(key.Id))))
                .V<Person>(new FancyId {Id = "someId"})
                .Should()
                .SerializeToGroovy("V(_a).hasLabel(_b).project('id', 'label', 'type', 'properties').by(id).by(label).by(__.constant('vertex')).by(__.properties().group().by(__.label()).by(__.project('id', 'label', 'value', 'properties').by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("someId", "Person");
        }

        [Fact]
        public void OverrideAtomSerializer_recognizes_derived_type()
        {
            g
                .UseModel(GraphModel
                    .FromBaseTypes<Vertex, Edge>())
                .ConfigureExecutionPipeline(_ => GremlinQueryExecutionPipeline
                    .EchoGroovy
                    .ConfigureSerializer(_ => _
                        .OverrideFragmentSerializer<FancyId>((key, overridden, recurse) => recurse(key.Id))))
                .V<Person>(new EvenMoreFancyId { Id = "someId" })
                .Should()
                .SerializeToGroovy("V(_a).hasLabel(_b).project('id', 'label', 'type', 'properties').by(id).by(label).by(__.constant('vertex')).by(__.properties().group().by(__.label()).by(__.project('id', 'label', 'value', 'properties').by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("someId", "Person");
        }

        [Fact]
        public void OverrideAtomSerializer_recognizes_interface()
        {
            g
                .UseModel(GraphModel
                    .FromBaseTypes<Vertex, Edge>())
                .ConfigureExecutionPipeline(_ => GremlinQueryExecutionPipeline
                    .EchoGroovy
                    .ConfigureSerializer(_ => _
                        .OverrideFragmentSerializer<IFancyId>((key, overridden, recurse) => recurse(key.Id))))
                .V<Person>(new FancyId { Id = "someId" })
                .Should()
                .SerializeToGroovy("V(_a).hasLabel(_b).project('id', 'label', 'type', 'properties').by(id).by(label).by(__.constant('vertex')).by(__.properties().group().by(__.label()).by(__.project('id', 'label', 'value', 'properties').by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("someId", "Person");
        }

        [Fact]
        public void OverrideAtomSerializer_recognizes_interface_through_derived_type()
        {
            g
                .UseModel(GraphModel
                    .FromBaseTypes<Vertex, Edge>())
                .ConfigureExecutionPipeline(_ => GremlinQueryExecutionPipeline
                    .EchoGroovy
                    .ConfigureSerializer(_ => _
                        .OverrideFragmentSerializer<IFancyId>((key, overridden, recurse) => recurse(key.Id))))
                .V<Person>(new FancyId { Id = "someId" })
                .Should()
                .SerializeToGroovy("V(_a).hasLabel(_b).project('id', 'label', 'type', 'properties').by(id).by(label).by(__.constant('vertex')).by(__.properties().group().by(__.label()).by(__.project('id', 'label', 'value', 'properties').by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("someId", "Person");
        }
    }
}
