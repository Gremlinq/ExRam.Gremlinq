﻿using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Tests.Infrastructure;
using Gremlin.Net.Structure;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class Issue568 : GremlinqTestBase
    {
        public abstract class VertexBaseAbstract
        {
            public object? Id { get; set; }
            public string? PartitionKey { get; set; }
        }

        public class Thing : VertexBaseAbstract
        {
        }

        public Issue568() : base(new DebugGremlinQueryVerifier())
        {

        }

        [Fact]
        public Task Repro() => g
            .ConfigureEnvironment(env => env
                .UseModel(GraphModel.FromBaseTypes<VertexBaseAbstract, Edge>()))
            .V<Thing>("id")
            .Update(new Thing())
            .Verify();
    }
}
