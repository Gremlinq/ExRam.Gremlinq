namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    public sealed class TrueExpressionSemantics : ConstantExpressionSemantics
    {
        public static readonly TrueExpressionSemantics Instance = new();

        private TrueExpressionSemantics()
        {

        }

        public override ExpressionSemantics Flip() => this;
    }

    public sealed class FalseExpressionSemantics : ConstantExpressionSemantics
    {
        public static readonly FalseExpressionSemantics Instance = new();

        private FalseExpressionSemantics()
        {

        }

        public override ExpressionSemantics Flip() => this; //TODO: Move into base when breaking API.
    }

    public abstract class ConstantExpressionSemantics : ExpressionSemantics
    {
    }
}
