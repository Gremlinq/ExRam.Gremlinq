using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Samples.Shared
{
    public class Edge : IEdge
    {
        public object? Id { get; set; }
        public string? Label { get; set; }
    }
}
