using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    internal static class ExpressionFragmentExtensions
    {
        public static object? GetValue(this ExpressionFragment expressionFragment, IGraphModel model)
        {
            return expressionFragment switch
            {
                ConstantExpressionFragment c => c.Value,
                { } x=> x.Expression?.GetValue(model),
                _ => throw new ArgumentException()
            };
        }
    }

    internal abstract class ExpressionFragment
    {
        public static readonly ConstantExpressionFragment True = new ConstantExpressionFragment(true);
        public static readonly ConstantExpressionFragment False = new ConstantExpressionFragment(default);
        public static readonly ConstantExpressionFragment Null = new ConstantExpressionFragment(default);

        protected ExpressionFragment(Expression? expression = default)
        {
            Expression = expression;
        }

        public Expression? Expression { get; }

        public static ExpressionFragment Create(Expression expression, IGraphModel model)
        {
            return expression.RefersToParameter()
                ? (ExpressionFragment)new ParameterExpressionFragment(expression)
                : expression.TryParseStepLabelExpression(model, out var stepLabel, out var stepLabelExpression)
                    ? new StepLabelExpressionFragment(stepLabel!, stepLabelExpression)
                    : new ConstantExpressionFragment(expression.GetValue(model));
        }
    }
}
