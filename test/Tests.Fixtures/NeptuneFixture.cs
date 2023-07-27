using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public sealed class NeptuneFixture : GremlinqFixture
    {
        public NeptuneFixture() : base(Core.GremlinQuerySource.g
            .UseNeptune(builder => builder
                .At(new Uri("ws://neptune:8182"))
                .UseNewtonsoftJson()))
        {
        }
    }
}
