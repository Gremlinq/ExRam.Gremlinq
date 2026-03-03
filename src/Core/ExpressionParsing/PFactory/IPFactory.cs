using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    /// <summary>Factory interface for creating TinkerPop <see cref="P"/> predicates from expression semantics.</summary>
    public interface IPFactory
    {
        /// <summary>Attempts to create a <see cref="P"/> predicate from the given semantics and value.</summary>
        /// <param name="semantics">The expression semantics.</param>
        /// <param name="value">The comparison value.</param>
        /// <param name="environment">The query environment.</param>
        P? TryGetP(ExpressionSemantics semantics, object? value, IGremlinQueryEnvironment environment);
    }
}
