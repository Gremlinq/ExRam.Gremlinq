using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class Edge : IEdge
    {
        public object Id { get; set; }
        public string Label { get; set; }
    }
}
