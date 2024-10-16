﻿using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public class SerializationTests : QueryExecutionTest, IClassFixture<NeptuneFixture>
    {
        public SerializationTests(NeptuneFixture fixture) : base(
            fixture,
            new SerializingVerifier<Bytecode>())
        {
        }
    }
}
