using System.Collections.Immutable;

using static ExRam.Gremlinq.Core.ExceptionHelper;

namespace ExRam.Gremlinq.Core.Serialization
{
    public readonly struct GroovyGremlinScript
    {
        private readonly string? _script;
        private readonly ImmutableDictionary<string, object>? _bindings;

        [Obsolete($"Use {nameof(GroovyGremlinScript)}({nameof(String)}, {nameof(ImmutableDictionary<string, object>)}) constructor instead.")]
        public GroovyGremlinScript(string script, IReadOnlyDictionary<string, object> bindings) : this(script, bindings.ToImmutableDictionary())
        {
        }

        public GroovyGremlinScript(string script, ImmutableDictionary<string, object> bindings)
        {
            _script = script;
            _bindings = bindings;
        }

        public override string ToString() => Script;

        public string Script => _script ?? throw UninitializedStruct();

        public ImmutableDictionary<string, object> Bindings => _bindings ?? throw UninitializedStruct();
    }
}
