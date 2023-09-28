namespace ExRam.Gremlinq.Templates.AspNet
{
    public class Edge
    {
#if (provider == "GremlinServer" || provider == "JanusGraph")
        public long? Id { get; set; }
#else
        public string? Id { get; set; }
#endif
    }
}
