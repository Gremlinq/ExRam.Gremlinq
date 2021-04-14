using System;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using Xunit;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.JanusGraph.Tests
{
    public sealed class JanusGraphQuerySerializationTest : QuerySerializationTest, IClassFixture<JanusGraphQuerySerializationTest.Fixture>
    {
        public sealed class Fixture : IntegrationTestFixture
        {
            public Fixture() : base(g
                .UseJanusGraph(builder => builder
                    .At(new Uri("ws://localhost:8182"))))
            {
            }
        }

        public JanusGraphQuerySerializationTest(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {

        }
    }
}
