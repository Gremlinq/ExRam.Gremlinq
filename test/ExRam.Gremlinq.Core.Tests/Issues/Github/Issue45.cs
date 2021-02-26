using System.Threading.Tasks;
using VerifyXunit;
using Xunit;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    [UsesVerify]
    public class Issue45 : VerifyBase
    {
        public Issue45() : base()
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
