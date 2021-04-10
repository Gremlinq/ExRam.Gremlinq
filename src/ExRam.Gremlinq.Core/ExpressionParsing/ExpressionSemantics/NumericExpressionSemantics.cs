namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    internal sealed class LowerThanExpressionSemantics : ObjectExpressionSemantics
    {
        public static readonly LowerThanExpressionSemantics Instance = new ();

        private LowerThanExpressionSemantics()
        {

        }

        public override ExpressionSemantics Flip() => GreaterThanExpressionSemantics.Instance;
    }

    internal sealed class LowerThanOrEqualExpressionSemantics : ObjectExpressionSemantics
    {
        public static readonly LowerThanOrEqualExpressionSemantics Instance = new();

        private LowerThanOrEqualExpressionSemantics()
        {

        }

        public override ExpressionSemantics Flip() => GreaterThanOrEqualExpressionSemantics.Instance;
    }

    internal sealed class GreaterThanOrEqualExpressionSemantics : ObjectExpressionSemantics
    {
        public static readonly GreaterThanOrEqualExpressionSemantics Instance = new();

        private GreaterThanOrEqualExpressionSemantics()
        {

        }

        public override ExpressionSemantics Flip() => LowerThanOrEqualExpressionSemantics.Instance;
    }

    internal sealed class GreaterThanExpressionSemantics : ObjectExpressionSemantics
    {
        public static readonly GreaterThanExpressionSemantics Instance = new();

        private GreaterThanExpressionSemantics()
        {

        }

        public override ExpressionSemantics Flip() => LowerThanExpressionSemantics.Instance;
    }
}
