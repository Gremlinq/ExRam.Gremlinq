using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Tests.Entities
{
    public abstract class Authority : Vertex
    {
        public VertexProperty<string, PropertyValidity> Name { get; set; }
    }
}
