using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class GroovyGremlinQuerySerializationTest : QueryExecutionTest
    {
        public GroovyGremlinQuerySerializationTest(ITestOutputHelper testOutputHelper) : base(
            g.ConfigureEnvironment(_ => _
                .UseSerializer(GremlinQuerySerializer.Default.ToGroovy())
                .UseExecutor(GremlinQueryExecutor.Echo)
                .UseDeserializer(GremlinQueryExecutionResultDeserializer.Identity)),
            testOutputHelper)
        {
        }
    }
}
