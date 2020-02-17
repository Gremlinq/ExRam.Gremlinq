using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Tests;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public class GremlinServerIntegrationTests : IntegrationTests
    {
        public GremlinServerIntegrationTests() : base(g.ConfigureEnvironment(env => env.UseWebSocket(_ => {})))
        {
        }
    }
}
