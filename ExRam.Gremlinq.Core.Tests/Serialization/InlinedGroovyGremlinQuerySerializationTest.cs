using Xunit.Abstractions;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class InlinedGroovyGremlinQuerySerializationTest : QuerySerializationTest
    {
        public InlinedGroovyGremlinQuerySerializationTest(ITestOutputHelper testOutputHelper) : base(
            GremlinQuerySource.g.ConfigureEnvironment(_ => _
                .UseSerializer(GremlinQuerySerializer.Default.ToGroovy(GroovyFormatting.AllowInlining))),
            testOutputHelper)
        {
        }
    }
}