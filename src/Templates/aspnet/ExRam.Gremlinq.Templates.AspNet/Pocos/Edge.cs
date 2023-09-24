namespace ExRam.Gremlinq.Templates.AspNet
{
    public class Edge
    {
#if (ProviderIsGremlinServer || ProviderIsJanusGraph)
        public long Id { get; set; }
#else
        public string Id { get; set; }
#endif
    }
}
