namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    public sealed class IntersectsExpressionSemantics : EnumerableExpressionSemantics
    {
        public static readonly IntersectsExpressionSemantics Instance = new();

        private IntersectsExpressionSemantics()
        {

        }

        public override ExpressionSemantics Flip() => this;
    }

    public sealed class ContainsExpressionSemantics : EnumerableExpressionSemantics
    {
        public static readonly ContainsExpressionSemantics Instance = new();

        private ContainsExpressionSemantics()
        {

        }

        public override ExpressionSemantics Flip() => IsContainedInExpressionSemantics.Instance;
    }

    public sealed class IsContainedInExpressionSemantics : EnumerableExpressionSemantics
    {
        public static readonly IsContainedInExpressionSemantics Instance = new();

        private IsContainedInExpressionSemantics()
        {

        }

        public override ExpressionSemantics Flip() => ContainsExpressionSemantics.Instance;
    }

    public abstract class EnumerableExpressionSemantics : ExpressionSemantics
    {
    }
}
