using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Tests.Entities
{
    public interface IAbstractVertex
    {
        VertexProperty<string, MetaModel>? Name { get; set; }
    }
}
