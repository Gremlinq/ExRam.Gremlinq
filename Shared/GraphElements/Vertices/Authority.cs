using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core.Tests
{
    public class MetaModel
    {
        public string MetaKey { get; set; }
    }

    public abstract class Authority : Vertex
    {
        public VertexProperty<string, MetaModel> Name { get; set; }
        public string[] PhoneNumbers { get; set; }
    }
}
