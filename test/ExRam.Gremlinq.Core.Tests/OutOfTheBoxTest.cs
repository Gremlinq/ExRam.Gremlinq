using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core.Deserialization;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Tests.Entities;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class OutOfTheBoxTest : GremlinqTestBase
    {
        private class SomeEntity
        {

        }

        public OutOfTheBoxTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public async Task Execution()
        {
            await g
                .Awaiting(_ => _
                    .ConfigureEnvironment(_ => _)
                    .V()
                    .ToArrayAsync())
                .Should()
                .ThrowAsync<InvalidOperationException>()
                .Where(x => x.Message.StartsWith("'Execute' must not be called on GremlinQueryExecutor.Invalid"));
        }

        [Fact]
        public async Task V_SomeEntity()
        {
            await g
                .ConfigureEnvironment(e => e
                    .UseModel(GraphModel
                        .FromBaseTypes<Vertex, Edge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes()))
                    .ConfigureSerializer(s => s.ToGroovy())
                    .UseExecutor(GremlinQueryExecutor.Identity)
                    .UseDeserializer(GremlinQueryExecutionResultDeserializer.Default))
                .V<SomeEntity>()
                .Cast<GroovyGremlinQuery>()
                .Verify();
        }

        public override IImmutableList<Func<string, string>> Scrubbers() => base
            .Scrubbers()
            .ScrubGuids();
    }
}
