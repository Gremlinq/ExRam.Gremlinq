namespace ExRam.Gremlinq.Templates.AspNet
{
    public class Vertex
    {
        public object? Id { get; set; }
        public string? Label { get; set; }

#if (ProviderIsCosmosDb)
        public string? PartitionKey { get; set; } = "PartitionKey";
#endif
    }
}
