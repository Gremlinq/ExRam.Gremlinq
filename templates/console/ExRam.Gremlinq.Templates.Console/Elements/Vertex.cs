namespace ExRam.Gremlinq.Templates.Console
{
    public class Vertex
    {
#if (actualProvider == "GremlinServer" || actualProvider == "JanusGraph")
        public long? Id { get; set; }
#else
        public string? Id { get; set; }
#endif
#if (actualProvider == "CosmosDb")

        public string? PartitionKey { get; set; } = "PartitionKey";
#endif
    }
}
