using System.Collections.Immutable;

using Gremlin.Net.Process.Traversal;

using static ExRam.Gremlinq.Core.ExceptionHelper;

namespace ExRam.Gremlinq.Core.Serialization
{
    /// <summary>
    /// Represents an intermediate Groovy expression with an identifier and a sequence of Gremlin instructions.
    /// </summary>
    public readonly struct GroovyExpression
    {
        private readonly string? _identifier;
        private readonly ImmutableArray<Instruction>? _instructions;

        private GroovyExpression(string identifier, ImmutableArray<Instruction> instructions)
        {
            if (identifier.Length == 0)
                throw new ArgumentException($"A {nameof(GroovyExpression)} must have a non-empty identifier.");

            _identifier = identifier;
            _instructions = instructions;
        }

        /// <summary>
        /// Creates a new <see cref="GroovyExpression"/> from an identifier and instructions.
        /// </summary>
        /// <param name="identifier">The expression identifier (e.g. <c>"g"</c>).</param>
        /// <param name="instructions">The sequence of Gremlin instructions.</param>
        public static GroovyExpression From(string identifier, ImmutableArray<Instruction> instructions)
        {
            ArgumentNullException.ThrowIfNull(identifier);

            return new(identifier, instructions);
        }

        /// <summary>
        /// Gets the identifier of this expression.
        /// </summary>
        public string Identifier => _identifier ?? throw UninitializedStruct();

        /// <summary>
        /// Gets the sequence of Gremlin instructions.
        /// </summary>
        public ImmutableArray<Instruction> Instructions => _instructions ?? throw UninitializedStruct();
    }
}
