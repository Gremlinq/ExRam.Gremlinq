using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Tests.Entities
{
    public abstract class Vertex : IVertex
    {
        public object Id { get; set; }

        public string Label { get; set; }
    }
}
