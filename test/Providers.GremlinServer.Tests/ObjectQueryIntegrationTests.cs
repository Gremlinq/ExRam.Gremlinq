﻿using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest("Linux")]
    [IntegrationTest("Windows")]
    public class ObjectQueryIntegrationTests : QueryExecutionTest, IClassFixture<GremlinServerContainerFixture>
    {
        public ObjectQueryIntegrationTests(GremlinServerContainerFixture fixture) : base(
            fixture,
            new ObjectQueryExecutingGremlinqVerifier())
        {
        }
    }
}
