﻿using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public class DebugTests : QueryExecutionTest, IClassFixture<CosmosDbFixture>
    {
        public DebugTests(CosmosDbFixture fixture) : base(
            fixture,
            new DebugGremlinQueryVerifier())
        {
        }
    }
}
