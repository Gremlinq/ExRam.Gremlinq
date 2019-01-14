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

        public static bool HasExpressionInMemberChain(this Expression expression, Expression searchedExpression)
        {
            while (true)
            {
                if (expression == searchedExpression)
                    return true;

                if (expression is MemberExpression memberExpression)
                {
                    expression = memberExpression.Expression;
                    continue;
                }

                return false;
            }
        }
    }
}
