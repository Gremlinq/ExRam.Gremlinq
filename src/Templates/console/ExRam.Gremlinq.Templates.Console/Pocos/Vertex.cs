namespace ExRam.Gremlinq.Templates.Console
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
