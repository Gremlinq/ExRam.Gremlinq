using System.Threading.Tasks;
using ExRam.Gremlinq.Core.Serialization;
using Xunit;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class Issue45 : GremlinqTestBase
    {
        public Issue45(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public async Task Repro()
        {
            await g
                .ConfigureEnvironment(env => env
                    .EchoGroovyGremlinQuery(GroovyFormatting.Inline))
                .V()
                .Drop()
                .Cast<string>()
                .Verify();
        }
    }
}
