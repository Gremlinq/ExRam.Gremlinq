using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Core.Tests.Fixtures;
using ExRam.Gremlinq.Providers.Core;

using Gremlin.Net.Driver.Messages;

using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class RequestMessageWithAliasSerializationTests : QueryExecutionTest, IClassFixture<RequestMessageWithAliasSerializationTests.RequestMessageWithAliasFixture>
    {
        public sealed class RequestMessageWithAliasFixture : SerializationFixture<RequestMessage>
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

        public RequestMessageWithAliasSerializationTests(RequestMessageWithAliasFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            GremlinQueryVerifier.Default,
            testOutputHelper)
        {

        }
    }
}
