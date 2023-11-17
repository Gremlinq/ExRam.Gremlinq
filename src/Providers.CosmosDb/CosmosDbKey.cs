namespace ExRam.Gremlinq.Providers.CosmosDb
{
    public readonly struct CosmosDbKey : IEquatable<CosmosDbKey>
    {
        private readonly string _id;

        public CosmosDbKey(string id) : this(default, id, default)
        {
        }

        public CosmosDbKey(string partitionKey, string id) : this(partitionKey, id, default)
        {
        }

        private CosmosDbKey(string? partitionKey, string id, bool dummy)
        {
            _id = id;
            PartitionKey = partitionKey;
        }

        public string Id
        {
            get
            {
                return _id ?? throw new InvalidOperationException($"Cannot access {nameof(Id)} property of an uninitialized {nameof(CosmosDbKey)}.");
            }
        }

        public string? PartitionKey { get; }

        public override bool Equals(object? obj) => obj is CosmosDbKey key && Equals(key);

        public bool Equals(CosmosDbKey other) => _id == other._id && PartitionKey == other.PartitionKey;

        public override int GetHashCode() => HashCode.Combine(_id, PartitionKey);

        public static bool operator ==(CosmosDbKey left, CosmosDbKey right) => left.Equals(right);

        public static bool operator !=(CosmosDbKey left, CosmosDbKey right) => !(left == right);
    }
}
