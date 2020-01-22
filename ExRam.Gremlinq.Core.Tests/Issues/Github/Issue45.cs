using Xunit;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class Issue45
    {
        [Fact]
        public void Repro()
        {
            g
                .ConfigureEnvironment(env => env
                .UseExecutionPipeline(GremlinQueryExecutionPipeline.EchoGroovy))
                .V()
                .Drop()
                .Should()
                .SerializeToGroovy("V().drop()")
                .WithoutParameters();
        }
    }
}
