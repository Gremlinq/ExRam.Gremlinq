using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    internal class ParameterExpressionFragment : ExpressionFragment
    {
        public ParameterExpressionFragment(Expression expression) : base(expression)
        {
        }
    }
}