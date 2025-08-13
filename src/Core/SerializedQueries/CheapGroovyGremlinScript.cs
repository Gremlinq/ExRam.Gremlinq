using static ExRam.Gremlinq.Core.ExceptionHelper;

namespace ExRam.Gremlinq.Core.Serialization
{
    internal readonly struct CheapGroovyGremlinScript
    {
        private readonly string? _script;

        private CheapGroovyGremlinScript(string script, IEnumerable<KeyValuePair<string, object?>>? bindings)
        {
            _script = script;
            Bindings = bindings;
        }

        public string Script => _script ?? throw UninitializedStruct();

        public IEnumerable<KeyValuePair<string, object?>>? Bindings { get; }

        public static CheapGroovyGremlinScript From(string script, IEnumerable<KeyValuePair<string, object?>>? bindings = null) => new(script, bindings);
    }
}
