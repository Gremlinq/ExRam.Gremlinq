namespace System.Linq.Expressions
{
    public static class ExpressionExtensions
    {
        public static Expression StripConvert(this Expression expression)
        {
            if (expression is UnaryExpression unaryExpression && expression.NodeType == ExpressionType.Convert)
                return unaryExpression.Operand.StripConvert();

            return expression;
        }
    }
}
