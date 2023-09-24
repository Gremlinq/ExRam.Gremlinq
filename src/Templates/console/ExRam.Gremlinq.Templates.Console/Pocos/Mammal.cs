using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Templates.Console
{
    public abstract class Mammal : Vertex
    {
        public int? Age { get; set; }

        public string? Name { get; set; }
    }
}
