using Xunit.Abstractions;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class InlinedLaterGroovyGremlinQuerySerializationTest : QuerySerializationTest
    {
        public InlinedLaterGroovyGremlinQuerySerializationTest(ITestOutputHelper testOutputHelper) : base(
            GremlinQuerySource.g.ConfigureEnvironment(_ => _
                .UseSerializer(GremlinQuerySerializer.Default.ToGroovy())
                .ConfigureSerializer(serializer => serializer.Select(query => ((GroovyGremlinQuery)query).Inline()))),
            testOutputHelper)
        {
        }
    }
}