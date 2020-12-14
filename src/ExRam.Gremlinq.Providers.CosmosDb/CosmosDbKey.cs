using System;

namespace ExRam.Gremlinq.Providers.CosmosDb
{
    public readonly struct CosmosDbKey
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
    }
}
