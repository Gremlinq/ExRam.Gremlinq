using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class RequestMessageWithAliasSerializationTests : QueryExecutionTestsBase, IClassFixture<RequestMessageWithAliasSerializationTests.Fixture>
    {
        public sealed class Fixture : GremlinServerFixture
        {
            protected override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g) => base
                .TransformQuerySource(g)
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(options => options
                        .SetValue(GremlinqOption.Alias, "a")))
                .IgnoreCosmosDbSpecificProperties();
        }

        public RequestMessageWithAliasSerializationTests(Fixture fixture) : base(
            fixture,
            new SerializingVerifier<RequestMessage>())
        {

        }
    }
}
