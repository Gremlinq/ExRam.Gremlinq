using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    internal static class ExpressionTypeExtensions
    {
        private static readonly IReadOnlyDictionary<ExpressionType, Func<object, P>> SupportedComparisons = new Dictionary<ExpressionType, Func<object, P>>
        {
            { ExpressionType.Equal, _ => P.Eq(_) },
            { ExpressionType.NotEqual, _ => P.Neq(_) },
            { ExpressionType.LessThan, _ => P.Lt(_) },
            { ExpressionType.LessThanOrEqual, _ => P.Lte(_) },
            { ExpressionType.GreaterThanOrEqual, _ => P.Gte(_) },
            { ExpressionType.GreaterThan, _ => P.Gt(_) }
        };

        public static P ToP(this ExpressionType expressionType, object argument)
        {
            return SupportedComparisons[expressionType](argument);
        }

        public static ExpressionType Switch(this ExpressionType expressionType)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
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
                    throw new ExpressionNotSupportedException($"The ExpressionType {expressionType} is not supported.");
            }
        }
    }
}
