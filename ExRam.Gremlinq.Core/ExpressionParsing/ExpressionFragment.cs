using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    internal abstract class ExpressionFragment
    {
        public static readonly ConstantExpressionFragment True = new ConstantExpressionFragment(true);
        public static readonly ConstantExpressionFragment Null = new ConstantExpressionFragment(default);

        protected ExpressionFragment(Expression? expression = default)
        {
            Expression = expression;
        }

        public Expression? Expression { get; }

        public static ExpressionFragment Create(Expression expression)
        {
            return expression.RefersToParameter()
                ? (ExpressionFragment)new ParameterExpressionFragment(expression)
                : expression.TryParseStepLabelExpression(out var stepLabel, out var stepLabelExpression)
                    ? new StepLabelExpressionFragment(stepLabel, stepLabelExpression)
                    : new ConstantExpressionFragment(expression.GetValue());
        }
    }
}