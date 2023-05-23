using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.Core;

using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class IntegrationGremlinServerFixture : GremlinqFixture
    {
        public IntegrationGremlinServerFixture() : base(g
            .UseGremlinServer(builder => builder
                .AtLocalhost()
                .UseNewtonsoftJson()))
        {
        }
    }
}
