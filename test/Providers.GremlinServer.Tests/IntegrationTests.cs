﻿using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Support.NewtonsoftJson.Tests;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public class IntegrationTests : QueryExecutionTest, IClassFixture<IntegrationTests.Fixture>
    {
        public new sealed class Fixture : IntegrationTestFixture
        {
            public Fixture() : base(g
                .UseGremlinServer(builder => builder
                    .AtLocalhost()
                    .UseNewtonsoftJson()))
            {
            }
        }

        public IntegrationTests(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
