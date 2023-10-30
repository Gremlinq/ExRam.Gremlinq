namespace ExRam.Gremlinq.Core.Serialization
{
    public readonly struct GroovyGremlinScript
    {
        private readonly string? _script;
        private readonly IReadOnlyDictionary<string, object>? _bindings;

        public GroovyGremlinScript(string script, IReadOnlyDictionary<string, object> bindings)
        {
            _script = script;
            _bindings = bindings;
        }

        public override string ToString() => Script;

        public string Script => _script ?? throw new InvalidOperationException();

        public IReadOnlyDictionary<string, object> Bindings => _bindings ?? throw new InvalidOperationException();
    }
}
