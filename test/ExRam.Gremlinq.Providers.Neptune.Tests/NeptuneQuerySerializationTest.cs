using System;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public class NeptuneQuerySerializationTest : QuerySerializationTest
    {
        public NeptuneQuerySerializationTest(ITestOutputHelper testOutputHelper) : base(
            g
                .ConfigureEnvironment(env => env
                    .UseNeptune(builder => builder
                        .At(new Uri("ws://localhost:8182")))),
            testOutputHelper)
        {
        }
    }
}
