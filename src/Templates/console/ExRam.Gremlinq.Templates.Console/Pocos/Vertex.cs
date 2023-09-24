namespace ExRam.Gremlinq.Templates.Console
{
    public class Vertex
    {
#if (ProviderIsGremlinServer)
        public long Id { get; set; }
#else
        public string Id { get; set; }
#endif

#if (ProviderIsCosmosDb)
        public string? PartitionKey { get; set; } = "PartitionKey";
#endif
    }
}
