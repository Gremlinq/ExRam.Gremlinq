using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public sealed class NeptuneFixture : GremlinqFixture
    {
        public NeptuneFixture() : base(g
            .UseNeptune(_ => _
                .AtLocalhost()
                .UseNewtonsoftJson()))
        {
        }
    }
}
