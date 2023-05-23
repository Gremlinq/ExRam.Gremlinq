using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.Core;

using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests.Fixtures
{
    public sealed class RequestMessageWithAliasFixture : GremlinqTestFixture
    {
        public RequestMessageWithAliasFixture() : base(g
            .UseGremlinServer(builder => builder
                .AtLocalhost())
            .ConfigureEnvironment(env => env
                .ConfigureOptions(options => options
                    .SetValue(GremlinqOption.Alias, "a"))))
        {
        }
    }
}
