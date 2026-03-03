namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    /// <summary>Represents less-than comparison semantics, translating to the Gremlin <c>lt</c> predicate.</summary>
    public sealed class LowerThanExpressionSemantics : ObjectExpressionSemantics
    {
        /// <summary>Gets the singleton instance.</summary>
        public static readonly LowerThanExpressionSemantics Instance = new ();

        private LowerThanExpressionSemantics()
        {

        }

        /// <inheritdoc />
        public override ExpressionSemantics Flip() => GreaterThanExpressionSemantics.Instance;
    }

    /// <summary>Represents less-than-or-equal comparison semantics, translating to the Gremlin <c>lte</c> predicate.</summary>
    public sealed class LowerThanOrEqualExpressionSemantics : ObjectExpressionSemantics
    {
        /// <summary>Gets the singleton instance.</summary>
        public static readonly LowerThanOrEqualExpressionSemantics Instance = new();

        private LowerThanOrEqualExpressionSemantics()
        {

        }

        /// <inheritdoc />
        public override ExpressionSemantics Flip() => GreaterThanOrEqualExpressionSemantics.Instance;
    }

    /// <summary>Represents greater-than-or-equal comparison semantics, translating to the Gremlin <c>gte</c> predicate.</summary>
    public sealed class GreaterThanOrEqualExpressionSemantics : ObjectExpressionSemantics
    {
        /// <summary>Gets the singleton instance.</summary>
        public static readonly GreaterThanOrEqualExpressionSemantics Instance = new();

        private GreaterThanOrEqualExpressionSemantics()
        {

        }

        /// <inheritdoc />
        public override ExpressionSemantics Flip() => LowerThanOrEqualExpressionSemantics.Instance;
    }

    /// <summary>Represents greater-than comparison semantics, translating to the Gremlin <c>gt</c> predicate.</summary>
    public sealed class GreaterThanExpressionSemantics : ObjectExpressionSemantics
    {
        /// <summary>Gets the singleton instance.</summary>
        public static readonly GreaterThanExpressionSemantics Instance = new();

        private GreaterThanExpressionSemantics()
        {

        }

        /// <inheritdoc />
        public override ExpressionSemantics Flip() => LowerThanExpressionSemantics.Instance;
    }
}
