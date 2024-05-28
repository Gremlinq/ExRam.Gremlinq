namespace ExRam.Gremlinq.Tests.Entities
{
    public abstract class Vertex
    {
        public object? Id { get; set; }

        public string? Label { get; set; }

        public string PartitionKey {get; set;} = nameof(PartitionKey);
    }
}
