﻿using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.Core;
using Gremlin.Net.Driver.Messages;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.JanusGraph.Tests
{
    public sealed class RequestMessageSerializationTests : SerializationTestsBase, IClassFixture<RequestMessageSerializationTests.RequestMessageFixture>
    {
        public sealed class RequestMessageFixture : SerializationTestsFixture<RequestMessage>
        {
            public RequestMessageFixture() : base(g
                .UseJanusGraph(builder => builder
                    .AtLocalhost()))
            {
            }
        }

        public RequestMessageSerializationTests(RequestMessageFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {

        }
    }
}
