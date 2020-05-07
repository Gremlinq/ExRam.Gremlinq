using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using VerifyXunit;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class Issue45 : VerifyBase
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
                .Verify(this);
        }
    }
}
