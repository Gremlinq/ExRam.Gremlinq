using System.Collections;
using System.Linq;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    internal class ConstantExpressionFragment : ExpressionFragment
    {
        public ConstantExpressionFragment(object? value, Expression? expression = default) : base(expression)
        {
            if (value is IEnumerable enumerable && !(value is ICollection) && !(value is string))
                value = enumerable.Cast<object>().ToArray();

            Value = value;
        }

        public object? Value { get; }
    }
}
