#if RELEASE && NET5_0 && RUNNEPTUNEINTEGRATIONTESTS
using System;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using Xunit.Abstractions;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public class NeptuneIntegrationTests : QueryIntegrationTest
    {
        public NeptuneIntegrationTests(ITestOutputHelper testOutputHelper) : base(
            GremlinQuerySource.g
                .UseNeptune(builder => builder
                    .At(new Uri("wss://127.0.0.1:8182"))),
            testOutputHelper)
        {
        }
    }
}
#endif
