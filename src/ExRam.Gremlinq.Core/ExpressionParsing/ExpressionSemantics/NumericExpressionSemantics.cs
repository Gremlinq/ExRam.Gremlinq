namespace ExRam.Gremlinq.Core
{
    internal sealed class LowerThanExpressionSemantics : NumericExpressionSemantics
    {
        public static readonly LowerThanExpressionSemantics Instance = new ();

        private LowerThanExpressionSemantics()
        {

        }

        public override ExpressionSemantics Flip() => GreaterThanExpressionSemantics.Instance;
    }

    internal sealed class LowerThanOrEqualExpressionSemantics : NumericExpressionSemantics
    {
        public static readonly LowerThanOrEqualExpressionSemantics Instance = new();

        private LowerThanOrEqualExpressionSemantics()
        {

        }

        public override ExpressionSemantics Flip() => GreaterThanOrEqualExpressionSemantics.Instance;
    }

    internal sealed class GreaterThanOrEqualExpressionSemantics : NumericExpressionSemantics
    {
        public static readonly GreaterThanOrEqualExpressionSemantics Instance = new();

        private GreaterThanOrEqualExpressionSemantics()
        {

        }

        public override ExpressionSemantics Flip() => LowerThanOrEqualExpressionSemantics.Instance;
    }

    internal sealed class GreaterThanExpressionSemantics : NumericExpressionSemantics
    {
        public static readonly GreaterThanExpressionSemantics Instance = new();

        private GreaterThanExpressionSemantics()
        {

        }

        public override ExpressionSemantics Flip() => LowerThanExpressionSemantics.Instance;
    }

    internal abstract class NumericExpressionSemantics : ObjectExpressionSemantics
    {

    }
}
