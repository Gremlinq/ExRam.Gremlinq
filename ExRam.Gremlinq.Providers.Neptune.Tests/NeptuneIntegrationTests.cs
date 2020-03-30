using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Tests;
using ExRam.Gremlinq.Providers.WebSocket;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public class NeptuneIntegrationTests : IntegrationTests
    {
        public NeptuneIntegrationTests() : base(g.ConfigureEnvironment(env => env.UseNeptune(builder => builder.AtLocalhost())))
        {
        }
    }
}
