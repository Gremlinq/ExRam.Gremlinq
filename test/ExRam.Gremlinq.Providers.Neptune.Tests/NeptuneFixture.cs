using System;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public sealed class NeptuneFixture : IIntegrationTestFixture
    {
        public NeptuneFixture()
        {
            GremlinQuerySource = Core.GremlinQuerySource.g
                .UseNeptune(builder => builder
                    .At(new Uri("wss://localhost:8182")));
        }

        public IGremlinQuerySource GremlinQuerySource { get; }
    }
}
