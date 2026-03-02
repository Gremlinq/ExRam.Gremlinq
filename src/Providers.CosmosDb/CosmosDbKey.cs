namespace ExRam.Gremlinq.Providers.CosmosDb
{
    /// <summary>
    /// Represents a key used to identify elements in CosmosDb, optionally including a partition key.
    /// </summary>
    public readonly struct CosmosDbKey : IEquatable<CosmosDbKey>
    {
        private readonly string _id;

        /// <summary>
        /// Initializes a new <see cref="CosmosDbKey"/> with the specified id.
        /// </summary>
        /// <param name="id">The element id.</param>
        public CosmosDbKey(string id) : this(null, id, false)
        {
            ArgumentNullException.ThrowIfNull(id);

        }

        /// <summary>
        /// Initializes a new <see cref="CosmosDbKey"/> with the specified partition key and id.
        /// </summary>
        /// <param name="partitionKey">The partition key.</param>
        /// <param name="id">The element id.</param>
        public CosmosDbKey(string partitionKey, string id) : this(partitionKey, id, false)
        {
            ArgumentNullException.ThrowIfNull(partitionKey);
            ArgumentNullException.ThrowIfNull(id);

        }

        private CosmosDbKey(string? partitionKey, string id, bool _)
        {
            _id = id;
            PartitionKey = partitionKey;
        }

        /// <summary>
        /// Gets the element id.
        /// </summary>
        public string Id => _id ?? throw new InvalidOperationException($"Cannot access {nameof(Id)} property of an uninitialized {nameof(CosmosDbKey)}.");

        /// <summary>
        /// Gets the partition key, or <c>null</c> if no partition key was specified.
        /// </summary>
        public string? PartitionKey { get; }

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is CosmosDbKey key && Equals(key);

        /// <inheritdoc />
        public bool Equals(CosmosDbKey other) => _id == other._id && PartitionKey == other.PartitionKey;

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(_id, PartitionKey);

        /// <summary>
        /// Determines whether two <see cref="CosmosDbKey"/> values are equal.
        /// </summary>
        public static bool operator ==(CosmosDbKey left, CosmosDbKey right) => left.Equals(right);

        /// <summary>
        /// Determines whether two <see cref="CosmosDbKey"/> values are not equal.
        /// </summary>
        public static bool operator !=(CosmosDbKey left, CosmosDbKey right) => !(left == right);
    }
}
