using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public sealed class GremlinServerFixture : GremlinqFixture
    {
        protected override async Task<IGremlinQuerySource> TransformQuerySource(IConfigurableGremlinQuerySource g) => g
            .UseGremlinServer(_ => _
                .AtLocalhost()
                .UseNewtonsoftJson());
    }
}
