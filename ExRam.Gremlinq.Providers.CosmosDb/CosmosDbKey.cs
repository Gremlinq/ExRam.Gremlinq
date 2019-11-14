using NullGuard;

namespace ExRam.Gremlinq.Core
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

        [AllowNull]
        public string? PartitionKey { get; }
    }
}
