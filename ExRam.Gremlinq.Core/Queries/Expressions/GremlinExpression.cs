using System.Linq.Expressions;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    internal struct GremlinExpression
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
