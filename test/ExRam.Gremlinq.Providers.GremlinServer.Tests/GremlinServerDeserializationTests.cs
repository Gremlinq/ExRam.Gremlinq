using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public class GremlinServerDeserializationTests : QueryDeserializationTest
    {
        public GremlinServerDeserializationTests(ITestOutputHelper testOutputHelper) : base(
            g.ConfigureEnvironment(env => env
                .ConfigureDeserializer(d => d
                    .ConfigureFragmentDeserializer(f => f
                        .AddNewtonsoftJson()))),
            testOutputHelper)
        {
        }
    }
}
