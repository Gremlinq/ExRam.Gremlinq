namespace ExRam.Gremlinq.Templates.AspNet
{
    public class Vertex
    {
#if (ProviderIsGremlinServer || ProviderIsJanusGraph)
        public long? Id { get; set; }
#else
        public string? Id { get; set; }
#endif
#if (ProviderIsCosmosDb)

        public string? PartitionKey { get; set; } = "PartitionKey";
#endif
    }
}
