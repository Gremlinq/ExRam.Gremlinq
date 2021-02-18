using System.Threading.Tasks;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    [UsesVerify]
    public class Issue45 : XunitContextBase
    {
        public Issue45(ITestOutputHelper output) : base(output)
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
                .Verify(this);
        }
    }
}
