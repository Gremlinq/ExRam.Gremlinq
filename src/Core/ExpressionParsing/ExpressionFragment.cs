using System.Collections;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    internal static class Expressions
    {
        public static readonly Expression True = ExpressionFragment.Create(Expression.Constant(true));
        public static readonly Expression False = ExpressionFragment.Create(Expression.Constant(false));
        public static readonly Expression Null = ExpressionFragment.Create(Expression.Constant(null, typeof(object)));
    }

    internal static class ExpressionFragment
    {
        public static Expression Create(Expression expression)
        {
            return expression.StripConvert();
        }
    }
}
