using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using Xunit;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class GremlinServerDeserializationTests : QueryDeserializationTest, IClassFixture<GremlinServerDeserializationTests.Fixture>
    {
        public new sealed class Fixture : QueryDeserializationTest.Fixture
        {
            public Fixture() : base(g
                .ConfigureEnvironment(env => env
                    .ConfigureDeserializer(d => d
                        .ConfigureFragmentDeserializer(f => f
                            .AddNewtonsoftJson()))))
            {
            }
        }

        public GremlinServerDeserializationTests(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
