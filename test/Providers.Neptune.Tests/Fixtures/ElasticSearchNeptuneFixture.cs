using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.Core;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public sealed class ElasticSearchNeptuneFixture : GremlinqTestFixture
    {
        public ElasticSearchNeptuneFixture() : base(g
            .UseNeptune(builder => builder
                .AtLocalhost()
                .UseElasticSearch(new Uri("http://elastic.search.server"))))
        {
        }
    }
}
