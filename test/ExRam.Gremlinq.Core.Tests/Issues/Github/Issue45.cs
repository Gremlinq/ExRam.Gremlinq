using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class Issue45 : GremlinqTestBase
    {
        public Issue45(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public Task Repro() => Verify(g
            .ConfigureEnvironment(env => env)
            .V()
            .Drop()
            .Debug());
    }
}
