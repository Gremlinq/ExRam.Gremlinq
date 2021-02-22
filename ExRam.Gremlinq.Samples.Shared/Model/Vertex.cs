using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Samples.Shared
{
    public class Vertex :  IVertex
    {
        public object? Id { get; set; }
        public string? Label { get; set; }
        public string PartitionKey { get; set; } = "PartitionKey";
    }
}
