using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Tests.Entities
{
    public abstract class Vertex : IVertex
    {
        public object Id { get; set; }
    }

    public class VertexWithStringId
    {
        public string Id { get; }
    }

    public class EdgeWithStringId
    {
        public string Id { get; }
    }

    public class VertexWithListAsId
    {
        public object[] Id { get; set; }
    }
}
