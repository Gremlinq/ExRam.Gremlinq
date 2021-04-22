using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.GremlinServer;
using Xunit;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public sealed class NeptuneQuerySerializationTest : QuerySerializationTest, IClassFixture<NeptuneQuerySerializationTest.Fixture>
    {
        public new sealed class Fixture : QuerySerializationTest.Fixture
        {
            public Fixture() : base(g
                .UseNeptune(builder => builder
                    .AtLocalhost()))
            {
            }
        }

        public NeptuneQuerySerializationTest(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
