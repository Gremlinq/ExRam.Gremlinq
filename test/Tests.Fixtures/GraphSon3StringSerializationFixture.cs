using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public sealed class GraphSon3StringSerializationFixture : GraphSonStringSerializationFixture
    {
        public GraphSon3StringSerializationFixture() : base(new GraphSON3Writer())
        {
        }
    }
}
