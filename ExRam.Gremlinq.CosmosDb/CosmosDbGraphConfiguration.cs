namespace ExRam.Gremlinq.CosmosDb
{
    public class CosmosDbGraphConfiguration
    {
        public string Hostname { get; set; }
        public int Port { get; set; } = 443;
        public bool EnableSsl { get; set; } = true;
        public string AuthKey { get; set; }
        public string Database { get; set; }
        public string GraphName { get; set; }
    }
}
