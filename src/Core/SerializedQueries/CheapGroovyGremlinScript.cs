using static ExRam.Gremlinq.Core.ExceptionHelper;

namespace ExRam.Gremlinq.Core.Serialization
{
    internal readonly struct CheapGroovyGremlinScript
    {
        private readonly string? _script;

        private CheapGroovyGremlinScript(string script, Bindings? bindings)
        {
            _script = script;
            Bindings = bindings;
        }

        public string Script => _script ?? throw UninitializedStruct();

        public Bindings? Bindings { get; }

        public static CheapGroovyGremlinScript From(string script, Bindings? bindings = null) => new(script, bindings);
    }
}
