namespace ExRam.Gremlinq.Core.Serialization
{
    public sealed class GroovyGremlinQuery
    {
        public GroovyGremlinQuery(string script, IReadOnlyDictionary<string, object> bindings) : this(Guid.NewGuid().ToString(), script, bindings)
        {
        }

        public GroovyGremlinQuery(string id, string script, IReadOnlyDictionary<string, object> bindings)
        {
            Id = id;
            Script = script;
            Bindings = bindings;
        }

        public override string ToString() => Script;

        public string Id { get; }

        public string Script { get; }

        public IReadOnlyDictionary<string, object> Bindings { get; }
    }
}
