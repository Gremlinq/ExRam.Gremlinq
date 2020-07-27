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
            public string? Id { get; set; }
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
            await g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<Vertex, Edge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes()))
                    .EchoGroovyString())
                .V<Person>()
                .Where(x => x.Age == 36)
                .Cast<string>()
                .Verify(this);
        }

        [Fact]
        public void Echo_wrong_type()
        {
            GremlinQuerySource.g
                .ConfigureEnvironment(env => env
                    .EchoGraphsonString())
                .V<Person>()
                .Awaiting(async _ => await _
                    .ToArrayAsync())
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Fact]
        public async Task OverrideAtomSerializer()
        {
            await g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<Vertex, Edge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes()))
                    .EchoGroovyString()
                    .ConfigureSerializer(_ => _
                        .ConfigureFragmentSerializer(fragmentSerializer => fragmentSerializer
                            .Override<FancyId>((key, env, overridden, recurse) => recurse.Serialize(key.Id, env)))))
                .V<Person>(new FancyId { Id = "someId" })
                .Verify(this);
        }

        [Fact]
        public async Task OverrideAtomSerializer_recognizes_derived_type()
        {
            await g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<Vertex, Edge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes()))
                    .EchoGroovyString()
                    .ConfigureSerializer(_ => _
                        .ConfigureFragmentSerializer(fragmentSerializer => fragmentSerializer
                            .Override<FancyId>((key, env, overridden, recurse) => recurse.Serialize(key.Id, env)))))
                .V<Person>(new EvenMoreFancyId { Id = "someId" })
                .Verify(this);
        }

        [Fact]
        public async Task OverrideAtomSerializer_recognizes_interface()
        {
            await g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<Vertex, Edge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes()))
                    .EchoGroovyString()
                    .ConfigureSerializer(_ => _
                        .ConfigureFragmentSerializer(fragmentSerializer => fragmentSerializer
                            .Override<IFancyId>((key, env, overridden, recurse) => recurse.Serialize(key.Id, env)))))
                .V<Person>(new FancyId { Id = "someId" })
                .Verify(this);
        }

        [Fact]
        public async Task OverrideAtomSerializer_recognizes_interface_through_derived_type()
        {
            await g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<Vertex, Edge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes()))
                    .EchoGroovyString()
                    .ConfigureSerializer(_ => _
                        .ConfigureFragmentSerializer(fragmentSerializer => fragmentSerializer
                            .Override<IFancyId>((key, env, overridden, recurse) => recurse.Serialize(key.Id, env)))))
                .V<Person>(new FancyId { Id = "someId" })
                .Verify(this);
        }
    }
}
