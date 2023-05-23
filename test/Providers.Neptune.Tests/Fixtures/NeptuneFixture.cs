using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public sealed class NeptuneFixture : GremlinqFixture
    {
        public NeptuneFixture() : base(Gremlinq.Core.GremlinQuerySource.g
            .UseNeptune(builder => builder
                .AtLocalhost())
            .ConfigureEnvironment(environment => environment
                .ConfigureExecutor(_ => _
                    .IgnoreResults())))
        {
        }
    }
}
