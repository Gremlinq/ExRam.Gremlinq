using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class Vertex : IVertex
    {
        public object Id { get; set; }
    }

    public class VertexWithListAsId
    {
        public object[] Id { get; set; }
    }
}
