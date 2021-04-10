using System;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.ExpressionParsing;

namespace ExRam.Gremlinq.Core
{
    internal static class ExpressionTypeExtensions
    {
        public static ExpressionSemantics ToSemantics(this ExpressionType type)
        {
            return type switch
            {
                ExpressionType.Equal => EqualsExpressionSemantics.Instance,
                ExpressionType.NotEqual => NotEqualsExpressionSemantics.Instance,
                ExpressionType.LessThan => LowerThanExpressionSemantics.Instance,
                ExpressionType.LessThanOrEqual => LowerThanOrEqualExpressionSemantics.Instance,
                ExpressionType.GreaterThanOrEqual => GreaterThanOrEqualExpressionSemantics.Instance,
                ExpressionType.GreaterThan => GreaterThanExpressionSemantics.Instance,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}
