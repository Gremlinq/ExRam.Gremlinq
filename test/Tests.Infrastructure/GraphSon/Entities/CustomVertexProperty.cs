using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Tests.Infrastructure.GraphSon.Entities
{
    public class CustomVertexProperty : VertexProperty<string>
    {
        public CustomVertexProperty(string value) : base(value)
        {
        }
    }
}
