namespace ExRam.Gremlinq.Core
{
    public sealed class CosmosDbKey
    {
        public CosmosDbKey(string partitionKey, string id)
        {
            PartitionKey = partitionKey;
            Id = id;
        }

        public string Id { get; }
        public string PartitionKey { get; }
    }
}