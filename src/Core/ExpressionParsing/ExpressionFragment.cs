using System.Collections;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    internal static class Expressions
    {
        public static readonly Expression True = Expression.Constant(true);
        public static readonly Expression False = Expression.Constant(false);
        public static readonly Expression Null = Expression.Constant(null, typeof(object));
    }
}
