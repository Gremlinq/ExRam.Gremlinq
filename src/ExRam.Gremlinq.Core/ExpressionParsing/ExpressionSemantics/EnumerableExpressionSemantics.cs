namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    internal sealed class IntersectsExpressionSemantics : EnumerableExpressionSemantics
    {
        public static readonly IntersectsExpressionSemantics Instance = new();

        private IntersectsExpressionSemantics()
        {

        }

        public override ExpressionSemantics Flip() => this;
    }

    internal sealed class ContainsExpressionSemantics : EnumerableExpressionSemantics
    {
        public static readonly ContainsExpressionSemantics Instance = new();

        private ContainsExpressionSemantics()
        {

        }

        public override ExpressionSemantics Flip() => IsContainedInExpressionSemantics.Instance;
    }

    internal sealed class IsContainedInExpressionSemantics : EnumerableExpressionSemantics
    {
        public static readonly IsContainedInExpressionSemantics Instance = new();

        private IsContainedInExpressionSemantics()
        {

        }

        public override ExpressionSemantics Flip() => ContainsExpressionSemantics.Instance;
    }

    internal abstract class EnumerableExpressionSemantics : ExpressionSemantics
    {
    }
}
