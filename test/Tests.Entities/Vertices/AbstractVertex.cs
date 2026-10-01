using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Tests.Entities
{
    public abstract class AbstractVertex : Vertex, IAbstractVertex
    {
        public VertexProperty<string, PropertyValidity>? Name { get; set; }
    }
}
