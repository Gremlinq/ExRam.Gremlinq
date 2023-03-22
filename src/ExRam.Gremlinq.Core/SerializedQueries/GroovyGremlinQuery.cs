namespace ExRam.Gremlinq.Core.Serialization
{
    public readonly struct GroovyGremlinQuery
    {
        private readonly string? _id;
        private readonly string? _script;
        private readonly IReadOnlyDictionary<string, object>? _bindings;

        public GroovyGremlinQuery(string script, IReadOnlyDictionary<string, object> bindings) : this(Guid.NewGuid().ToString(), script, bindings)
        {
        }

        public GroovyGremlinQuery(string id, string script, IReadOnlyDictionary<string, object> bindings)
        {
            _id = id;
            _script = script;
            _bindings = bindings;
        }

        public override string ToString() => Script;

        public string Id => _id ?? throw new InvalidOperationException();

        public string Script => _script ?? throw new InvalidOperationException();

        public IReadOnlyDictionary<string, object> Bindings => _bindings ?? throw new InvalidOperationException();
    }
}
