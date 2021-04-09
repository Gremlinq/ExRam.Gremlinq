namespace ExRam.Gremlinq.Core
{
    internal sealed class TrueExpressionSemantics : ConstantExpressionSemantics
    {
        public static readonly TrueExpressionSemantics Instance = new();

        private TrueExpressionSemantics()
        {

        }

        public override ExpressionSemantics Flip() => FalseExpressionSemantics.Instance;
    }

    internal sealed class FalseExpressionSemantics : ConstantExpressionSemantics
    {
        public static readonly FalseExpressionSemantics Instance = new();

        private FalseExpressionSemantics()
        {

        }

        public override ExpressionSemantics Flip() => TrueExpressionSemantics.Instance;
    }

    internal abstract class ConstantExpressionSemantics : ExpressionSemantics
    {
    }
}
