using System;
using System.Threading.Tasks;
using ExRam.Gremlinq.Tests.Entities;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using VerifyXunit;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class ExecutionPipelinesTest : VerifyBase
    {
        private interface IFancyId
        {
            string Id { get; set; }
        }

        private class FancyId : IFancyId
        {
            public string Id { get; set; }
        }

        public ExecutionPipelinesTest(ITestOutputHelper output) : base(output)
        {

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
                    .EchoGroovy())
                .V<Person>()
                .Where(x => x.Age == 36)
                .Cast<string>()
                .FirstAsync();

            query
                .Should()
                .Be("V().hasLabel(_a).has(_b, _c).limit(_d).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))");
        }

        [Fact]
        public void Echo_wrong_type()
        {
            GremlinQuerySource.g
                .ConfigureEnvironment(env => env
                    .EchoGraphson())
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
                    .EchoGroovy()
                    .ConfigureSerializer(_ => _
                        .OverrideFragmentSerializer<FancyId>((key, overridden, recurse) => recurse(key.Id))))
                .V<Person>(new FancyId { Id = "someId" })
                .VerifyQuery(this);
        }

        [Fact]
        public void OverrideAtomSerializer_recognizes_derived_type()
        {
            g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<Vertex, Edge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes()))
                    .EchoGroovy()
                    .ConfigureSerializer(_ => _
                        .OverrideFragmentSerializer<FancyId>((key, overridden, recurse) => recurse(key.Id))))
                .V<Person>(new EvenMoreFancyId { Id = "someId" })
                .VerifyQuery(this);
        }

        [Fact]
        public void OverrideAtomSerializer_recognizes_interface()
        {
            g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<Vertex, Edge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes()))
                    .EchoGroovy()
                    .ConfigureSerializer(_ => _
                        .OverrideFragmentSerializer<IFancyId>((key, overridden, recurse) => recurse(key.Id))))
                .V<Person>(new FancyId { Id = "someId" })
                .VerifyQuery(this);
        }

        [Fact]
        public void OverrideAtomSerializer_recognizes_interface_through_derived_type()
        {
            g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<Vertex, Edge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes()))
                    .EchoGroovy()
                    .ConfigureSerializer(_ => _
                        .OverrideFragmentSerializer<IFancyId>((key, overridden, recurse) => recurse(key.Id))))
                .V<Person>(new FancyId { Id = "someId" })
                .VerifyQuery(this);
        }
    }
}
