using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Entities;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public sealed class NeptuneFixture : GremlinqFixture
    {
        protected override async Task<IGremlinQuerySource> TransformQuerySource(IConfigurableGremlinQuerySource g) => g
            .UseNeptune<Vertex, Edge>(_ => _
                .AtLocalhost()
                .UseNewtonsoftJson());
    }
}
