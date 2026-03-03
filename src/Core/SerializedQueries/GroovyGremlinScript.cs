using System.Collections.Immutable;

using static ExRam.Gremlinq.Core.ExceptionHelper;

namespace ExRam.Gremlinq.Core.Serialization
{
    /// <summary>
    /// Represents a Gremlin query serialized as a Groovy script with optional bindings.
    /// </summary>
    public readonly struct GroovyGremlinScript
    {
        private readonly string? _script;
        private readonly ImmutableDictionary<string, object?>? _bindings;

        private GroovyGremlinScript(string script, ImmutableDictionary<string, object?>? bindings)
        {
            _script = script;
            _bindings = bindings ?? ImmutableDictionary<string, object?>.Empty;
        }

        /// <summary>
        /// Returns a new script with the specified binding added.
        /// </summary>
        /// <param name="variable">The binding variable name.</param>
        /// <param name="value">The binding value.</param>
        public GroovyGremlinScript Bind(string variable, object? value)
        {
            ArgumentNullException.ThrowIfNull(variable);

            return new(Script, Bindings.SetItem(variable, value));
        }

        public override string ToString() => Script;

        /// <summary>
        /// Gets the Groovy script text.
        /// </summary>
        public string Script => _script ?? throw UninitializedStruct();

        /// <summary>
        /// Gets the bindings (parameter name to value mappings) for the script.
        /// </summary>
        public ImmutableDictionary<string, object?> Bindings => _bindings ?? throw UninitializedStruct();

        /// <summary>
        /// Creates a new <see cref="GroovyGremlinScript"/> from a script string and optional bindings.
        /// </summary>
        /// <param name="script">The Groovy script text.</param>
        /// <param name="bindings">Optional bindings for the script.</param>
        public static GroovyGremlinScript From(string script, ImmutableDictionary<string, object?>? bindings = null)
        {
            ArgumentNullException.ThrowIfNull(script);

            return new (script, bindings);
        }
    }
}
