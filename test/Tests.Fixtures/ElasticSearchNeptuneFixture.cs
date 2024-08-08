using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public class ElasticSearchNeptuneFixture : GremlinqFixture
    {
        protected override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g) => g
            .UseNeptune<Vertex, Edge>(builder => builder
                .AtLocalhost()
                .UseElasticSearch(new Uri("http://elastic.search.server")))
            .IgnoreCosmosDbSpecificProperties();
    }
}
