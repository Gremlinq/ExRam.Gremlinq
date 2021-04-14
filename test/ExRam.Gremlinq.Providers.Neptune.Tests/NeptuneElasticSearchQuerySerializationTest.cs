using System;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using Xunit;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public sealed class NeptuneElasticSearchQuerySerializationTest : QuerySerializationTest, IClassFixture<NeptuneElasticSearchQuerySerializationTest.Fixture>
    {
        public sealed class Fixture : IntegrationTestFixture
        {
            public Fixture() : base(g
                .UseNeptune(builder => builder
                    .At(new Uri("ws://localhost:8182"))
                    .UseElasticSearch(new Uri("http://elastic.search.server"))))
            {
            }
        }

        public NeptuneElasticSearchQuerySerializationTest(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
