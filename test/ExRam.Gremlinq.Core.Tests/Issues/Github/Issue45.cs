using System.Threading.Tasks;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    [UsesVerify]
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
                    .EchoGroovyString())
                .V()
                .Drop()
                .Cast<string>()
                .Verify();
        }
    }
}
