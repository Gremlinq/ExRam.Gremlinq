namespace System.Linq.Expressions
{
    internal static class ExpressionExtensions
    {
        public static Expression StripConvert(this Expression expression)
        {
            if (expression is UnaryExpression unaryExpression && expression.NodeType == ExpressionType.Convert)
            {
                // ReSharper disable once TailRecursiveCall
                return unaryExpression.Operand.StripConvert();
            }

            return expression;
        }

        public static object GetValue(this Expression expression)
        {
            if (expression is ConstantExpression constantExpression)
                return constantExpression.Value;

            return Expression
                .Lambda<Func<object>>(Expression.Convert(expression, typeof(object)))
                .Compile()();
        }
    }
}
