using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Tests.Entities
{
    public interface IAuthority
    {
        VertexProperty<string, PropertyValidity>? Name { get; set; }
    }
}
