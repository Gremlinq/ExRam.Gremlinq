using System;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Tests;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public class NeptuneIntegrationTests : IntegrationTests
    {
        public NeptuneIntegrationTests() : base(g.ConfigureEnvironment(env => env.UseNeptune(new Uri("ws://localhost:8182"))))
        {
        }
    }
}
