using System;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Tests.Entities;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class ExecutionPipelinesTest : GremlinqTestBase
    {
        private interface IFancyId
        {
            string? Id { get; set; }
        }

        private class FancyId : IFancyId
        {
            public string? Id { get; set; }
        }

        public ExecutionPipelinesTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        private class EvenMoreFancyId : FancyId
        {
        }

        [Fact]
        public async Task Echo()
        {
            await g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<Vertex, Edge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes()))
                    .EchoGroovyGremlinQuery())
                .V<Person>()
                .Where(x => x.Age == 36)
                .Cast<string>()
                .Verify();
        }

        [Fact]
        public async Task Echo_wrong_type()
        {
            await g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<Vertex, Edge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes()))
                    .EchoGroovyGremlinQuery())
                .V<Person>()
                .Awaiting(_ => _
                    .ToArrayAsync())
                .Should()
                .ThrowAsync<InvalidCastException>();
        }

        [Fact]
        public async Task OverrideAtomSerializer()
        {
            await g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<Vertex, Edge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes()))
                    .EchoGroovyGremlinQuery()
                    .ConfigureSerializer(_ => _
                        .ConfigureFragmentSerializer(fragmentSerializer => fragmentSerializer
                            .Override<FancyId>((key, env, overridden, recurse) => recurse.Serialize(key.Id, env)))))
                .V<Person>(new FancyId { Id = "someId" })
                .Cast<string>()
                .Verify();
        }

        [Fact]
        public async Task OverrideAtomSerializer_recognizes_derived_type()
        {
            await g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<Vertex, Edge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes()))
                    .EchoGroovyGremlinQuery()
                    .ConfigureSerializer(_ => _
                        .ConfigureFragmentSerializer(fragmentSerializer => fragmentSerializer
                            .Override<FancyId>((key, env, overridden, recurse) => recurse.Serialize(key.Id, env)))))
                .V<Person>(new EvenMoreFancyId { Id = "someId" })
                .Cast<string>()
                .Verify();
        }

        [Fact]
        public async Task OverrideAtomSerializer_recognizes_interface()
        {
            await g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<Vertex, Edge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes()))
                    .EchoGroovyGremlinQuery()
                    .ConfigureSerializer(_ => _
                        .ConfigureFragmentSerializer(fragmentSerializer => fragmentSerializer
                            .Override<IFancyId>((key, env, overridden, recurse) => recurse.Serialize(key.Id, env)))))
                .V<Person>(new FancyId { Id = "someId" })
                .Cast<string>()
                .Verify();
        }

        [Fact]
        public async Task OverrideAtomSerializer_recognizes_interface_through_derived_type()
        {
            await g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<Vertex, Edge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes()))
                    .EchoGroovyGremlinQuery()
                    .ConfigureSerializer(_ => _
                        .ConfigureFragmentSerializer(fragmentSerializer => fragmentSerializer
                            .Override<IFancyId>((key, env, overridden, recurse) => recurse.Serialize(key.Id, env)))))
                .V<Person>(new FancyId { Id = "someId" })
                .Cast<string>()
                .Verify();
        }
    }
}
