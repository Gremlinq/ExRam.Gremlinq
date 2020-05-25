using System.Collections;
using System.Linq;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    internal class ConstantExpressionFragment : ExpressionFragment
    {
        public ConstantExpressionFragment(object? value, Expression? expression = default) : base(expression)
        {
            //TODO: References string. Should work for any native type.
            if (value is IEnumerable enumerable && !(value is ICollection) && !(value is string))
                value = enumerable.Cast<object>().ToArray();

            Value = value;
        }

        public object? Value { get; }
    }
}
