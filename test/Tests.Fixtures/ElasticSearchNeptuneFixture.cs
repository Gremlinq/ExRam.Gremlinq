using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Fixtures;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public sealed class ElasticSearchNeptuneFixture : GremlinqFixture
    {
        public ElasticSearchNeptuneFixture() : base(g
            .UseNeptune(builder => builder
                .AtLocalhost()
                .UseElasticSearch(new Uri("http://elastic.search.server"))))
        {
        }
    }
}
