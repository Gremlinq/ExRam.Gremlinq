namespace ExRam.Gremlinq.Providers.CosmosDb
{
    public sealed class CosmosDbKey
    {
        public CosmosDbKey(string partitionKey, string id) : this(id)
        {
            PartitionKey = partitionKey;
        }

        public CosmosDbKey(string id)
        {
            Id = id;
        }

        public string Id { get; }

        public string? PartitionKey { get; }
    }
}
