using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.Core;

using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.Neptune.Tests.Fixtures
{
    public sealed class SimpleNeptuneFixture : GremlinqFixture
    {
        public SimpleNeptuneFixture() : base(g
            .UseNeptune(builder => builder
                .AtLocalhost()))
        {
        }
    }
}
