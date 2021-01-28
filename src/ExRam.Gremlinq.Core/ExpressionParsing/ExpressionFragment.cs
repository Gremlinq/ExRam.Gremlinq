using System.Collections;
using System.Linq;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    internal abstract class ExpressionFragment
    {
        public static readonly ConstantExpressionFragment True = new(true);
        public static readonly ConstantExpressionFragment False = new(false);
        public static readonly ConstantExpressionFragment Null = new(default);

        protected ExpressionFragment(Expression? expression = default)
        {
            Expression = expression;
        }

        public Expression? Expression { get; }

        public static ExpressionFragment Create(Expression expression, IGraphModel model)
        {
            return expression.RefersToParameter()
                ? (ExpressionFragment)new ParameterExpressionFragment(expression)
                : expression.TryParseStepLabelExpression(out var stepLabel, out var stepLabelExpression)
                    ? new ConstantExpressionFragment(stepLabel!, stepLabelExpression)
                    : new ConstantExpressionFragment(expression.GetValue() switch
                    {
                        IEnumerable enumerable when !(enumerable is ICollection) && !model.NativeTypes.Contains(enumerable.GetType()) => enumerable.Cast<object>().ToArray(),
                        { } val => val,
                        _ => null
                    });
        }
    }
}
