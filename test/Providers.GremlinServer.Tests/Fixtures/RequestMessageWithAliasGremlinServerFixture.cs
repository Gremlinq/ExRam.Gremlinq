using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.Core;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class RequestMessageWithAliasGremlinServerFixture : GremlinqFixture
    {
        public RequestMessageWithAliasGremlinServerFixture() : base(g
            .UseGremlinServer(builder => builder
                .AtLocalhost())
            .ConfigureEnvironment(env => env
                .ConfigureOptions(options => options
                    .SetValue(GremlinqOption.Alias, "a"))))
        {
        }
    }
}
