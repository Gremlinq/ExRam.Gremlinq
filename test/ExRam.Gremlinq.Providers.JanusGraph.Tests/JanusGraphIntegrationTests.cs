#if RELEASE && RUNJANUSGRAPHINTEGRATIONTESTS
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.WebSocket;
using Xunit;
using Xunit.Abstractions;

namespace ExRam.Gremlinq.Providers.JanusGraph.Tests
{
    public class JanusGraphIntegrationTests : QueryIntegrationTest, IClassFixture<JanusGraphIntegrationTests.Fixture>
    {
        public new sealed class Fixture : QueryIntegrationTest.Fixture
        {
            public Fixture() : base(Gremlinq.Core.GremlinQuerySource.g
                .UseJanusGraph(builder => builder
                    .At(new Uri("ws://localhost:8183")))
                .ConfigureEnvironment(environment => environment
                    .ConfigureExecutor(_ => _
                        .TransformResult(_ => AsyncEnumerable.Empty<object>()))))
            {
            }
        }

        private static readonly Regex RelationIdRegex = new("\"relationId\":[\\s]?\"[0-9a-z]{3}([-][0-9a-z]{3})*\"", RegexOptions.IgnoreCase);

        public JanusGraphIntegrationTests(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }

        [Fact(Skip = "ServerError: Expected valid relation id: id")]
        public override Task Properties_Where_Id()
        {
            return base.Properties_Where_Id();
        }

        [Fact(Skip = "ServerError: Expected valid relation id: id")]
        public override Task Properties_Where_Id_equals_static_field()
        {
            return base.Properties_Where_Id_equals_static_field();
        }

        public override IImmutableList<Func<string, string>> Scrubbers()
        {
            return base.Scrubbers()
                .Add(x => RelationIdRegex.Replace(x, "\"relationId\": \"scrubbed\""));
        }
    }
}
#endif
