namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    /// <summary>Represents collection intersection semantics, translating to a collection-level predicate.</summary>
    public sealed class IntersectsExpressionSemantics : EnumerableExpressionSemantics
    {
        /// <summary>Gets the singleton instance.</summary>
        public static readonly IntersectsExpressionSemantics Instance = new();

        private IntersectsExpressionSemantics()
        {

        }

        /// <inheritdoc />
        public override ExpressionSemantics Flip() => this;
    }

    /// <summary>Represents collection contains semantics, translating to the Gremlin <c>within</c> predicate.</summary>
    public sealed class ContainsExpressionSemantics : EnumerableExpressionSemantics
    {
        /// <summary>Gets the singleton instance.</summary>
        public static readonly ContainsExpressionSemantics Instance = new();

        private ContainsExpressionSemantics()
        {

        }

        /// <inheritdoc />
        public override ExpressionSemantics Flip() => IsContainedInExpressionSemantics.Instance;
    }

    /// <summary>Represents "is contained in" semantics, the flipped form of <see cref="ContainsExpressionSemantics"/>.</summary>
    public sealed class IsContainedInExpressionSemantics : EnumerableExpressionSemantics
    {
        /// <summary>Gets the singleton instance.</summary>
        public static readonly IsContainedInExpressionSemantics Instance = new();

        private IsContainedInExpressionSemantics()
        {

        }

        /// <inheritdoc />
        public override ExpressionSemantics Flip() => ContainsExpressionSemantics.Instance;
    }

    /// <summary>Base class for expression semantics operating on enumerable/collection types.</summary>
    public abstract class EnumerableExpressionSemantics : ExpressionSemantics;
}
