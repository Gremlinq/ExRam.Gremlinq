using System;
using System.Threading.Tasks;

using ExRam.Gremlinq.Core.Models;

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
            await g
                .ConfigureEnvironment(_ => _
                    .UseModel(GraphModel.Empty))
                .WithExecutor("[\"2021-03-31T14:57:20.3482309Z\"]")
                .V<string>()
                .Verify();
        }

        [Fact]
        public async Task Deserialize_to_DateTime()
        {
            await g
                .ConfigureEnvironment(_ => _
                    .UseModel(GraphModel.Empty))
                .WithExecutor("[\"2021-03-31T14:57:20.3482309Z\"]")
                .V<DateTime>()
                .Verify();
        }
    }
}
