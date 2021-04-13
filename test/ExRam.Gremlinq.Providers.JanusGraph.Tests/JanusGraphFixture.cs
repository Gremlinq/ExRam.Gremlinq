using System;
using System.Linq;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;

namespace ExRam.Gremlinq.Providers.JanusGraph.Tests
{
    public sealed class JanusGraphFixture : IIntegrationTestFixture
    {
        public JanusGraphFixture()
        {
            GremlinQuerySource = Core.GremlinQuerySource.g
                .UseJanusGraph(builder => builder
                    .At(new Uri("ws://localhost:8183")))
                .ConfigureEnvironment(environment => environment
                    .ConfigureExecutor(_ => _
                        .TransformResult(_ => AsyncEnumerable.Empty<object>())));
        }

        public IGremlinQuerySource GremlinQuerySource { get; }
    }
}
