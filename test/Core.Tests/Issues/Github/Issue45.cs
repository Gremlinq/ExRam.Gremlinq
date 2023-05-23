using ExRam.Gremlinq.Tests.Infrastructure;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class Issue45 : GremlinqTestBase
    {
        public Issue45() : base(new DebugGremlinQueryVerifier())
        {

        }

        [Fact]
        public Task Repro() => g
            .ConfigureEnvironment(env => env)
            .V()
            .Drop()
            .Verify();
    }
}
