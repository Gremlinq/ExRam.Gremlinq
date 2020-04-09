using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Structure.IO.GraphSON;
using Xunit.Abstractions;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class Graphson2GremlinQuerySerializationTest : QuerySerializationTest
    {
        private static readonly GraphSON2Writer Writer = new GraphSON2Writer();

        public Graphson2GremlinQuerySerializationTest(ITestOutputHelper testOutputHelper) : base(
            GremlinQuerySource.g.ConfigureEnvironment(_ => _
                .UseSerializer(GremlinQuerySerializer.Default
                    .Select(obj => Writer.WriteObject((Bytecode)obj)))),
            testOutputHelper)
        {
        }
    }
}