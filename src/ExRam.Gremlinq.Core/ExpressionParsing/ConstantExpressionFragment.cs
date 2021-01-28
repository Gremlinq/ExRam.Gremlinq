using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    internal sealed class ConstantExpressionFragment : ExpressionFragment
    {
        public ConstantExpressionFragment(object? value, Expression? expression = default) : base(expression)
        {
            Value = value;
        }

        public object? Value { get; }
    }
}
