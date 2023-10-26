using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using ExRam.Gremlinq.Providers.Neptune;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public sealed class NeptuneFixture : GremlinqFixture
    {
        protected override async Task<IGremlinQuerySource> TransformQuerySource(IGremlinQuerySource g) => g
            .UseNeptune<Vertex, Edge>(_ => _
                .AtLocalhost()
                .UseNewtonsoftJson());
    }
}
