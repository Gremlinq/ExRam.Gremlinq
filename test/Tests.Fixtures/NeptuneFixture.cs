using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Fixtures;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public sealed class NeptuneFixture : GremlinqFixture
    {
        public NeptuneFixture() : base(Gremlinq.Core.GremlinQuerySource.g
            .UseNeptune(builder => builder
                .At(new Uri("ws://localhost:8184"))
                .UseNewtonsoftJson()))
        {
        }
    }
}
