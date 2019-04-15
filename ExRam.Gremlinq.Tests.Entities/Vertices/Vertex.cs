using ExRam.Gremlinq.Core.Attributes;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Tests.Entities
{
    public abstract class Vertex : IVertex
    {
        [ReadOnly]
        public object Id { get; set; }
    }

    public class VertexWithListAsId
    {
        public object[] Id { get; set; }
    }
}
