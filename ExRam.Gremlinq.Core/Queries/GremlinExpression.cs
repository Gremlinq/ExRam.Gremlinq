namespace ExRam.Gremlinq.Core
{
    internal readonly struct GremlinExpression
    {
        public GremlinExpression(ExpressionFragment left, ExpressionSemantics semantics, ExpressionFragment right)
        {
            if (!(left is ParameterExpressionFragment) && right is ParameterExpressionFragment)
            {
                Left = right;
                Semantics = semantics.Flip();
                Right = left;
            }
            else
            {
                Left = left;
                Right = right;
                Semantics = semantics;
            }
        }

        public ExpressionFragment Left { get; }
        public ExpressionFragment Right { get; }
        public ExpressionSemantics Semantics { get; }
    }
}
