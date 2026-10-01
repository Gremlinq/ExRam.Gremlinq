using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Tests.Entities
{
    public class PropertyEdge : Edge
    {
        public Property<DateTimeOffset>? Since { get; set; }
    }
}
