namespace ExRam.Gremlinq.Samples.Shared
{
    public class Vertex
    {
        public object? Id { get; set; }
        public string? Label { get; set; }
        public string PartitionKey { get; set; } = "PartitionKey";
    }
}
