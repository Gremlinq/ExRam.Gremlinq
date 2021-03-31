using System.Threading.Tasks;

using ExRam.Gremlinq.Providers.Tests;

using Xunit;
using Xunit.Abstractions;

using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class Issue219 : GremlinqTestBase
    {
        public Issue219(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public async Task Repro()
        {
            var expected = "2021-03-31T14:57:20.3482309Z";

            var actual = await g
                .ConfigureEnvironment(_ => _)
                .WithExecutor($"[\"{expected}\"]")
                .V<string>()
                .SingleAsync();

            Assert.Equal(expected, actual);
        }
    }
}
