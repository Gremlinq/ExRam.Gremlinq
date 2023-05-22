using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Support.NewtonsoftJson.Tests;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class DeserializationTests : QueryExecutionTest, IClassFixture<DeserializationTests.DeserializationFixture>
    {
        public sealed class DeserializationFixture : GremlinqTestFixture
        {
            public DeserializationFixture() : base(g
                .UseGremlinServer(_ => _
                   .UseNewtonsoftJson()))
            {
            }
        }

        public DeserializationTests(DeserializationFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new DeserializingGremlinqVerifier(),
            testOutputHelper)
        {
        }
    }
}
