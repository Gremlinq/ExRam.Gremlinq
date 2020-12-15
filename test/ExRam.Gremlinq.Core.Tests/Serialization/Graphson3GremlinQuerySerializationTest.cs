using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Structure.IO.GraphSON;
using Xunit.Abstractions;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class Graphson3GremlinQuerySerializationTest : QuerySerializationTest
    {
        private static readonly GraphSON3Writer Writer = new();

        public Graphson3GremlinQuerySerializationTest(ITestOutputHelper testOutputHelper) : base(
            GremlinQuerySource.g.ConfigureEnvironment(_ => _
                .UseSerializer(GremlinQuerySerializer.Default
                    .Select(obj => Writer.WriteObject((Bytecode)obj)))),
            testOutputHelper)
        {
        }
    }
}
