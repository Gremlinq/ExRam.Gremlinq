using System;
using System.Linq;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    internal static class ExpressionSemanticsExtensions
    {
        private static readonly P P_Neq_Null = P.Neq(new object[] { null });

        public static ExpressionSemantics Flip(this ExpressionSemantics semantics)
        {
            return semantics switch
            {
                ExpressionSemantics.Contains => ExpressionSemantics.IsContainedIn,
                ExpressionSemantics.StartsWith => ExpressionSemantics.IsPrefixOf,
                ExpressionSemantics.EndsWith => ExpressionSemantics.IsSuffixOf,
                ExpressionSemantics.HasInfix => ExpressionSemantics.IsInfixOf,
                ExpressionSemantics.LowerThan => ExpressionSemantics.GreaterThan,
                ExpressionSemantics.GreaterThan => ExpressionSemantics.LowerThan,
                ExpressionSemantics.Equals => ExpressionSemantics.Equals,
                ExpressionSemantics.Intersects => ExpressionSemantics.Intersects,
                ExpressionSemantics.GreaterThanOrEqual => ExpressionSemantics.LowerThanOrEqual,
                ExpressionSemantics.LowerThanOrEqual => ExpressionSemantics.GreaterThanOrEqual,
                ExpressionSemantics.NotEquals => ExpressionSemantics.NotEquals,
                ExpressionSemantics.IsContainedIn => ExpressionSemantics.Contains,
                ExpressionSemantics.IsInfixOf => ExpressionSemantics.HasInfix,
                ExpressionSemantics.IsPrefixOf => ExpressionSemantics.StartsWith,
                ExpressionSemantics.IsSuffixOf => ExpressionSemantics.EndsWith,
                _ => throw new ArgumentOutOfRangeException(nameof(semantics), semantics, null)
            };
        }

        public static P ToP(this ExpressionSemantics semantics, object? value)
        {
            return semantics switch
            {
                ExpressionSemantics.Contains => P.Eq(value),
                ExpressionSemantics.IsPrefixOf when value is string stringValue => P.Within(Enumerable
                    .Range(0, stringValue.Length + 1)
                    .Select(i => stringValue.Substring(0, i))
                    .ToArray<object>()),
                ExpressionSemantics.HasInfix when value is string stringValue => stringValue.Length > 0
                    ? TextP.Containing(stringValue)
                    : P_Neq_Null,
                ExpressionSemantics.StartsWith when value is string stringValue => stringValue.Length > 0
                    ? TextP.StartingWith(stringValue)
                    : P_Neq_Null,
                ExpressionSemantics.EndsWith when value is string stringValue => stringValue.Length > 0
                    ? TextP.EndingWith(stringValue)
                    : P_Neq_Null,
                ExpressionSemantics.LowerThan => P.Lt(value),
                ExpressionSemantics.GreaterThan => P.Gt(value),
                ExpressionSemantics.Equals => P.Eq(value),
                ExpressionSemantics.NotEquals => P.Neq(value),
                ExpressionSemantics.Intersects => P.Within(value),
                ExpressionSemantics.GreaterThanOrEqual => P.Gte(value),
                ExpressionSemantics.LowerThanOrEqual => P.Lte(value),
                ExpressionSemantics.IsContainedIn => P.Within(value),
                ExpressionSemantics.IsInfixOf => throw new ExpressionNotSupportedException(),
                ExpressionSemantics.IsSuffixOf => throw new ExpressionNotSupportedException(),
                _ => throw new ArgumentOutOfRangeException(nameof(semantics), semantics, null)
            };
        }
    }
}