#if RELEASE && NET5_0 && RUNJANUSGRAPHINTEGRATIONTESTS
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.WebSocket;

using Newtonsoft.Json.Linq;

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
                        .AtLocalhost())
                    .ConfigureExecutor(x => x.TransformResult(_ => new[]{ JToken.Parse("\"nondeterministic\"") }.ToAsyncEnumerable()))),
            testOutputHelper)
        {
        }

        public override IImmutableList<Func<string, string>> Scrubbers()
        {
            return base.Scrubbers()
                .Add(x => RelationIdRegex.Replace(x, "\"relationId\": \"scrubbed\""));
        }
    }
}
#endif
