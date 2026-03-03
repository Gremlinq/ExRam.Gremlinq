namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    /// <summary>Represents equality comparison semantics, translating to the Gremlin <c>eq</c> predicate.</summary>
    public sealed class EqualsExpressionSemantics : ObjectExpressionSemantics
    {
        /// <summary>Gets the singleton instance.</summary>
        public static readonly EqualsExpressionSemantics Instance = new ();

        private EqualsExpressionSemantics()
        {

        }

        /// <inheritdoc />
        public override ExpressionSemantics Flip() => this;
    }

    /// <summary>Represents inequality comparison semantics, translating to the Gremlin <c>neq</c> predicate.</summary>
    public sealed class NotEqualsExpressionSemantics : ObjectExpressionSemantics
    {
        /// <summary>Gets the singleton instance.</summary>
        public static readonly NotEqualsExpressionSemantics Instance = new();

        private NotEqualsExpressionSemantics()
        {

        }

        /// <inheritdoc />
        public override ExpressionSemantics Flip() => this;
    }

    /// <summary>Base class for expression semantics operating on object comparisons.</summary>
    public abstract class ObjectExpressionSemantics : ExpressionSemantics
    {
        /// <summary>Transforms this semantics for a <c>CompareTo</c> result comparison.</summary>
        /// <param name="comparison">The constant integer value compared against the <c>CompareTo</c> result.</param>
        public ExpressionSemantics TransformCompareTo(int comparison) => this switch
        {
            LowerThanExpressionSemantics => comparison switch
            {
                0 => LowerThanExpressionSemantics.Instance,
                1 => LowerThanOrEqualExpressionSemantics.Instance,
                > 1 => TrueExpressionSemantics.Instance,
                _ => FalseExpressionSemantics.Instance
            },
            LowerThanOrEqualExpressionSemantics => comparison switch
            {
                -1 => LowerThanExpressionSemantics.Instance,
                0 => LowerThanOrEqualExpressionSemantics.Instance,
                > 0 => TrueExpressionSemantics.Instance,
                _ => FalseExpressionSemantics.Instance
            },
            EqualsExpressionSemantics => comparison switch
            {
                -1 => LowerThanExpressionSemantics.Instance,
                0 => EqualsExpressionSemantics.Instance,
                1 => GreaterThanExpressionSemantics.Instance,
                _ => FalseExpressionSemantics.Instance
            },
            GreaterThanOrEqualExpressionSemantics => comparison switch
            {
                <= -1 => TrueExpressionSemantics.Instance,
                0 => GreaterThanOrEqualExpressionSemantics.Instance,
                1 => GreaterThanExpressionSemantics.Instance,
                _ => FalseExpressionSemantics.Instance
            },
            GreaterThanExpressionSemantics => comparison switch
            {
                < -1 => TrueExpressionSemantics.Instance,
                -1 => GreaterThanOrEqualExpressionSemantics.Instance,
                0 => GreaterThanExpressionSemantics.Instance,
                _ => FalseExpressionSemantics.Instance
            },
            NotEqualsExpressionSemantics => comparison switch
            {
                -1 => GreaterThanOrEqualExpressionSemantics.Instance,
                0 => NotEqualsExpressionSemantics.Instance,
                1 => LowerThanOrEqualExpressionSemantics.Instance,
                _ => TrueExpressionSemantics.Instance
            },
            _ => throw new ExpressionNotSupportedException()
        };
    }
}
