using System;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public class NeptuneElasticSearchQuerySerializationTest : QuerySerializationTest
    {
        public NeptuneElasticSearchQuerySerializationTest(ITestOutputHelper testOutputHelper) : base(
            g
                .UseNeptune(builder => builder
                    .At(new Uri("ws://localhost:8182"))
                    .UseElasticSearch(new Uri("http://elastic.search.server"))),
            testOutputHelper)
        {
        }
    }
}
