namespace ExRam.Gremlinq.Templates.Console
{
    public class Edge
    {
#if (actualProvider == "GremlinServer" || actualProvider == "JanusGraph")
        public long? Id { get; set; }
#else
        public string? Id { get; set; }
#endif
    }
}
