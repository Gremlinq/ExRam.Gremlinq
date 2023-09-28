namespace ExRam.Gremlinq.Templates.AspNet
{
    public class Vertex
    {
#if (provider == "GremlinServer" || provider == "JanusGraph")
        public long? Id { get; set; }
#else
        public string? Id { get; set; }
#endif
#if (provider == "CosmosDb")

        public string? PartitionKey { get; set; } = "PartitionKey";
#endif
    }
}
