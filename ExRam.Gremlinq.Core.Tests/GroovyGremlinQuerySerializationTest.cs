using Xunit.Abstractions;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class GroovyGremlinQuerySerializationTest : QuerySerializationTest
    {
        public GroovyGremlinQuerySerializationTest(ITestOutputHelper testOutputHelper) : base(
            GremlinQuerySource.g
                .ConfigureEnvironment(env => env
                    .ConfigureSerializer(ser => ser
                        .ToGroovy())),
            testOutputHelper)
        {
        }
    }
}