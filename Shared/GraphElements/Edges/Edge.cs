using ExRam.Gremlinq.GraphElements;

namespace ExRam.Gremlinq.Tests
{
    public abstract class Edge : IEdge
    {
        public object Id { get; set; }
        public string Label { get; set; }
    }
}
