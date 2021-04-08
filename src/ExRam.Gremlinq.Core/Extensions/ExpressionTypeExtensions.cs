using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    internal static class ExpressionTypeExtensions
    {
        public static ExpressionSemantics ToSemantics(this ExpressionType type)
        {
            return type switch
            {
                ExpressionType.Equal => ObjectExpressionSemantics.Equals,
                ExpressionType.NotEqual => ObjectExpressionSemantics.NotEquals,
                ExpressionType.LessThan => NumericExpressionSemantics.LowerThan,
                ExpressionType.LessThanOrEqual => NumericExpressionSemantics.LowerThanOrEqual,
                ExpressionType.GreaterThanOrEqual => NumericExpressionSemantics.GreaterThanOrEqual,
                ExpressionType.GreaterThan => NumericExpressionSemantics.GreaterThan,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}
