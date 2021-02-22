using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Samples.Shared
{
    public abstract class Mammal : Vertex
    {
        public int Age { get; set; }

        public VertexProperty<string, NameMeta>? Name { get; set; }
    }
}
