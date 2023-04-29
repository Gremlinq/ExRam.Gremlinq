namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    public sealed class LowerThanExpressionSemantics : ObjectExpressionSemantics
    {
        public static readonly LowerThanExpressionSemantics Instance = new ();

        private LowerThanExpressionSemantics()
        {

        }

        public override ExpressionSemantics Flip() => GreaterThanExpressionSemantics.Instance;
    }

    public sealed class LowerThanOrEqualExpressionSemantics : ObjectExpressionSemantics
    {
        public static readonly LowerThanOrEqualExpressionSemantics Instance = new();

        private LowerThanOrEqualExpressionSemantics()
        {

        }

        public override ExpressionSemantics Flip() => GreaterThanOrEqualExpressionSemantics.Instance;
    }

    public sealed class GreaterThanOrEqualExpressionSemantics : ObjectExpressionSemantics
    {
        public static readonly GreaterThanOrEqualExpressionSemantics Instance = new();

        private GreaterThanOrEqualExpressionSemantics()
        {

        }

        public override ExpressionSemantics Flip() => LowerThanOrEqualExpressionSemantics.Instance;
    }

    public sealed class GreaterThanExpressionSemantics : ObjectExpressionSemantics
    {
        public static readonly GreaterThanExpressionSemantics Instance = new();

        private GreaterThanExpressionSemantics()
        {

        }

        public override ExpressionSemantics Flip() => LowerThanExpressionSemantics.Instance;
    }
}
