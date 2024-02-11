namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    public sealed class TrueExpressionSemantics : ConstantExpressionSemantics
    {
        public static readonly TrueExpressionSemantics Instance = new();

        private TrueExpressionSemantics()
        {

        }
    }

    public sealed class FalseExpressionSemantics : ConstantExpressionSemantics
    {
        public static readonly FalseExpressionSemantics Instance = new();

        private FalseExpressionSemantics()
        {

        }
    }

    public abstract class ConstantExpressionSemantics : ExpressionSemantics
    {
        public override ExpressionSemantics Flip() => this;
    }
}
