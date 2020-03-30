using System;
using System.Linq;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.WebSocket;
using ExRam.Gremlinq.Tests.Entities;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public class GremlinServerGroovySerializationTest : GroovySerializationTest
    {
        public GremlinServerGroovySerializationTest(ITestOutputHelper testOutputHelper) : base(
            g
                .ConfigureEnvironment(env => env
                .UseGremlinServer(builder => builder
                    .AtLocalhost())),
            testOutputHelper)
        {

        }

        [Fact]
        public void AddV_with_enum_property_with_workaround()
        {
            _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(options => options
                        .SetItem(GremlinServerGremlinqOptions.WorkaroundTinkerpop2112, true)))
                .AddV(new Person { Id = 1, Gender = Gender.Female })
                .VerifyQuery(this);
        }
    }
}
