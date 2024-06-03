using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Tests.Fixtures;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public sealed class ElasticSearchNeptuneFixture : GremlinqFixture
    {
        protected override async Task<IGremlinQuerySource> TransformQuerySource(IGremlinQuerySource g) => g
            .UseNeptune<Vertex, Edge>(builder => builder
                .AtLocalhost()
                .UseElasticSearch(new Uri("http://elastic.search.server")))
            .IgnoreCosmosDbSpecificProperties();
    }
}
