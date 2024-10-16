﻿using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public class RequestMessageWithAliasGremlinServerFixture : GremlinqFixture
    {
        protected override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g) => g
            .UseGremlinServer<Vertex, Edge>(builder => builder
                .AtLocalhost())
            .ConfigureEnvironment(env => env
                .ConfigureOptions(options => options
                    .SetValue(GremlinqOption.Alias, "a")))
            .IgnoreCosmosDbSpecificProperties();
    }
}
