using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class InlinedGroovyGremlinQuerySerializationTest : QuerySerializationTest
    {
        public InlinedGroovyGremlinQuerySerializationTest(ITestOutputHelper testOutputHelper) : base(
            g.ConfigureEnvironment(_ => _
                .UseSerializer(GremlinQuerySerializer.Default.ToGroovy(GroovyFormatting.Inline))),
            testOutputHelper)
        {
        }
    }
}
