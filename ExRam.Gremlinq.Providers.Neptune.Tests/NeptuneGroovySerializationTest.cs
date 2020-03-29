using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Providers.WebSocket;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public class NeptuneGroovySerializationTest : GroovySerializationTest
    {
        public NeptuneGroovySerializationTest(ITestOutputHelper testOutputHelper) : base(
            g
                .ConfigureEnvironment(env => env
                    .UseNeptune(builder => builder.AtLocalhost())),
            testOutputHelper)
        {
        }
    }
}
