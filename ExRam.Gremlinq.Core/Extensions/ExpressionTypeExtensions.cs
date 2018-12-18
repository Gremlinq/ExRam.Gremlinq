using System.Collections.Generic;
using ExRam.Gremlinq.Core;

namespace System.Linq.Expressions
{
    internal static class ExpressionTypeExtensions
    {
        private static readonly IReadOnlyDictionary<ExpressionType, Func<object, P>> SupportedComparisons = new Dictionary<ExpressionType, Func<object, P>>
        {
            { ExpressionType.Equal, _ => new P.Eq(_) },
            { ExpressionType.NotEqual, _ => new P.Neq(_) },
            { ExpressionType.LessThan, _ => new P.Lt(_) },
            { ExpressionType.LessThanOrEqual, _ => new P.Lte(_) },
            { ExpressionType.GreaterThanOrEqual, _ => new P.Gte(_) },
            { ExpressionType.GreaterThan, _ => new P.Gt(_) }
        };

        public static P ToP(this ExpressionType expressionType, object argument)
        {
            return SupportedComparisons[expressionType](argument);
        }

        public static ExpressionType Switch(this ExpressionType expressionType)
        {
            switch (expressionType)
            {
                case ExpressionType.Equal:
                    return expressionType;
                case ExpressionType.Not:
                    return expressionType;
                case ExpressionType.LessThan:
                    return ExpressionType.GreaterThan;
                case ExpressionType.LessThanOrEqual:
                    return ExpressionType.GreaterThanOrEqual;
                case ExpressionType.GreaterThanOrEqual:
                    return ExpressionType.LessThanOrEqual;
                case ExpressionType.GreaterThan:
                    return ExpressionType.LessThan;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
