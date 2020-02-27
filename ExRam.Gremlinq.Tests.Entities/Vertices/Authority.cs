using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Tests.Entities
{
    public class EntityWithTwoIntProperties
    {
        public int IntProperty1 { get; set; }
        public int IntProperty2 { get; set; }
    }

    public interface IAuthority
    {
        VertexProperty<string, PropertyValidity> Name { get; set; }
    }

    public abstract class Authority : Vertex, IAuthority
    {
        public VertexProperty<string, PropertyValidity> Name { get; set; }
    }
}
