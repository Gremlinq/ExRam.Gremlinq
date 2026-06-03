namespace ExRam.Gremlinq.Templates.Console
{
    public class Vertex
    {
        public string? Id { get; set; }
#if (provider == "CosmosDb")

        public string? PartitionKey { get; set; } = "PartitionKey";
#endif
    }
}
