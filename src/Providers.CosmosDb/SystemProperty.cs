namespace ExRam.Gremlinq.Providers.CosmosDb
{
    public readonly struct SystemProperty
    {
        public static readonly SystemProperty _ts = new(nameof(_ts));
        public static readonly SystemProperty _rid = new(nameof(_rid));
        public static readonly SystemProperty _etag = new(nameof(_etag));
        public static readonly SystemProperty _self = new(nameof(_self));
        public static readonly SystemProperty inVPartition = new(nameof(inVPartition));
        public static readonly SystemProperty outVPartition = new(nameof(outVPartition));

        private readonly string? _name;

        private SystemProperty(string name)
        {
            _name = name;
        }

        public string Name => _name ?? throw new InvalidOperationException();
    }
}
