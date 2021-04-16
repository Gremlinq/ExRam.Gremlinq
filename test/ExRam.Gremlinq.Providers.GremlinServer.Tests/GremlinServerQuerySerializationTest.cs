using System;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Tests.Entities;
using Xunit;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class GremlinServerQuerySerializationTest : QuerySerializationTest, IClassFixture<GremlinServerQuerySerializationTest.Fixture>
    {
        public new sealed class Fixture : QuerySerializationTest.Fixture
        {
            public Fixture() : base(g
                .UseGremlinServer(builder => builder
                    .At(new Uri("ws://localhost:8182"))))
            {
            }
        }

        public GremlinServerQuerySerializationTest(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {

        }

        [Fact]
        public async Task AddV_with_enum_property_with_workaround()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(options => options
                        .SetValue(GremlinServerGremlinqOptions.WorkaroundTinkerpop2112, true)))
                .AddV(new Person { Id = 1, Gender = Gender.Female })
                .Verify();
        }
    }
}
