#if RELEASE && NET5_0 && RUNJANUSGRAPHINTEGRATIONTESTS
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.WebSocket;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.JanusGraph.Tests
{
    public class JanusGraphIntegrationTests : QueryIntegrationTest
    {
        private static readonly Regex RelationIdRegex = new("\"relationId\":[\\s]?\"[0-9a-z]{3}([-][0-9a-z]{3})*\"", RegexOptions.IgnoreCase);

        public JanusGraphIntegrationTests(ITestOutputHelper testOutputHelper) : base(
            g
                .ConfigureEnvironment(env => env
                    .UseJanusGraph(builder => builder
                        .At("ws://localhost:8183"))
                    .ConfigureExecutor(_ => _
                        .TransformResult(_ => AsyncEnumerable.Empty<object>()))),
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
