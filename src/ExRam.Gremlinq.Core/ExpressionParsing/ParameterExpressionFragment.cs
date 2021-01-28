using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    internal sealed class ParameterExpressionFragment : ExpressionFragment
    {
        public ParameterExpressionFragment(Expression expression) : base(expression)
        {
        }
    }
}
