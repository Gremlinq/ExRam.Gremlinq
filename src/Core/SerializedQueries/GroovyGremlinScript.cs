using System.Collections.Immutable;

using static ExRam.Gremlinq.Core.ExceptionHelper;

namespace ExRam.Gremlinq.Core.Serialization
{
    public readonly struct GroovyGremlinScript
    {
        private readonly string? _script;
        private readonly ImmutableDictionary<string, object?>? _bindings;

        [Obsolete($"Use {nameof(GroovyGremlinScript)}.{nameof(From)}(...) instead.")]
        public GroovyGremlinScript(string script, IReadOnlyDictionary<string, object?> bindings) : this(script, bindings.ToImmutableDictionary())
        {
        }

        private GroovyGremlinScript(string script, ImmutableDictionary<string, object?>? bindings)
        {
            _script = script;
            _bindings = bindings ?? ImmutableDictionary<string, object?>.Empty;
        }

        public GroovyGremlinScript Bind(string variable, object? value) => new(Script, Bindings.SetItem(variable, value));

        public override string ToString() => Script;

        public string Script => _script ?? throw UninitializedStruct();

        public ImmutableDictionary<string, object?> Bindings => _bindings ?? throw UninitializedStruct();

        public static GroovyGremlinScript From(string script, ImmutableDictionary<string, object?>? bindings = null) => new (script, bindings);
    }
}
