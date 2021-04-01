using System;
using System.Globalization;
using System.Threading.Tasks;

using ExRam.Gremlinq.Providers.Tests;

using Newtonsoft.Json;

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
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                DateParseHandling = DateParseHandling.None
            };

            await g
                .ConfigureEnvironment(_ => _)
                .WithExecutor("[\"2021-03-31T14:57:20.3482309Z\"]")
                .V<string>()
                .Verify();
        }

        [Fact]
        public async Task Deserialize_to_DateTime()
        {
            await g
                .ConfigureEnvironment(_ => _)
                .WithExecutor("[\"2021-03-31T14:57:20.3482309Z\"]")
                .V<DateTime>()
                .Verify();
        }
    }
}
