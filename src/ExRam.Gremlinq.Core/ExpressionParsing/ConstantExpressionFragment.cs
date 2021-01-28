using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    internal sealed class ConstantExpressionFragment : ExpressionFragment
    {
        private readonly object? _value;

        public ConstantExpressionFragment(object? value, Expression? expression = default) : base(expression)
        {
            _value = value;
        }

        public override object? GetValue() => _value;
    }
}
