using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Tests.Entities
{
    public abstract class Authority : Vertex, IAuthority
    {
        public VertexProperty<string, PropertyValidity> Name { get; set; }
    }
}
