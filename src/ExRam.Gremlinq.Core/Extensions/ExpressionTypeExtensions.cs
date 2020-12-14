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
                ExpressionType.Equal => ExpressionSemantics.Equals,
                ExpressionType.NotEqual => ExpressionSemantics.NotEquals,
                ExpressionType.LessThan => ExpressionSemantics.LowerThan,
                ExpressionType.LessThanOrEqual => ExpressionSemantics.LowerThanOrEqual,
                ExpressionType.GreaterThanOrEqual => ExpressionSemantics.GreaterThanOrEqual,
                ExpressionType.GreaterThan => ExpressionSemantics.GreaterThan,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}
