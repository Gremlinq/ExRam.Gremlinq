namespace ExRam.Gremlinq.Providers.CosmosDb
{
    /// <summary>
    /// Represents a CosmosDb system property that is automatically managed by the database.
    /// </summary>
    public readonly struct SystemProperty
    {
        /// <summary>The <c>_ts</c> system property (timestamp).</summary>
        public static readonly SystemProperty _ts = new(nameof(_ts));
        /// <summary>The <c>_rid</c> system property (resource id).</summary>
        public static readonly SystemProperty _rid = new(nameof(_rid));
        /// <summary>The <c>_etag</c> system property (entity tag).</summary>
        public static readonly SystemProperty _etag = new(nameof(_etag));
        /// <summary>The <c>_self</c> system property (self link).</summary>
        public static readonly SystemProperty _self = new(nameof(_self));
        /// <summary>The <c>inVPartition</c> system property (incoming vertex partition key).</summary>
        public static readonly SystemProperty inVPartition = new(nameof(inVPartition));
        /// <summary>The <c>outVPartition</c> system property (outgoing vertex partition key).</summary>
        public static readonly SystemProperty outVPartition = new(nameof(outVPartition));

        private readonly string? _name;

        private SystemProperty(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Gets the name of the system property.
        /// </summary>
        public string Name => _name ?? throw new InvalidOperationException();
    }
}
