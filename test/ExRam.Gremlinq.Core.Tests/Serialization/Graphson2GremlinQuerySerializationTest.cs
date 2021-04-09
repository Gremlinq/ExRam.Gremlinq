using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Structure.IO.GraphSON;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class Graphson2GremlinQuerySerializationTest : QuerySerializationTest
    {
        private static readonly GraphSON2Writer Writer = new();

        public Graphson2GremlinQuerySerializationTest(ITestOutputHelper testOutputHelper) : base(
            g.ConfigureEnvironment(_ => _
                .UseSerializer(GremlinQuerySerializer.Default
                    .Select(obj => Writer.WriteObject((Bytecode)obj)))),
            testOutputHelper)
        {
        }
    }
}
