namespace ExRam.Gremlinq.CosmosDb
{
    public class CosmosDbGraphConfiguration
    {
        public string EndPoint { get; set; }
        public string AuthKey { get; set; }
        public string Database { get; set; }
        public string GraphName { get; set; }
        public string TraversalSource { get; set; }
    }
}
