using System;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    internal static class ExpressionSemanticsExtensions
    {
        private static readonly P PNeqNull = P.Neq(new object?[] { null });

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
                ExpressionSemantics.Contains => new P("eq", value),
                ExpressionSemantics.IsPrefixOf when value is string stringValue => P.Within(SubStrings(stringValue)),
                ExpressionSemantics.HasInfix when value is string stringValue => stringValue.Length > 0
                    ? TextP.Containing(stringValue)
                    : PNeqNull,
                ExpressionSemantics.StartsWith when value is string stringValue => stringValue.Length > 0
                    ? TextP.StartingWith(stringValue)
                    : PNeqNull,
                ExpressionSemantics.EndsWith when value is string stringValue => stringValue.Length > 0
                    ? TextP.EndingWith(stringValue)
                    : PNeqNull,
                ExpressionSemantics.LowerThan => new P("lt", value),
                ExpressionSemantics.GreaterThan => new P("gt", value),
                ExpressionSemantics.Equals => new P("eq", value),
                ExpressionSemantics.NotEquals => new P("neq", value),
                ExpressionSemantics.Intersects => P.Within(value),
                ExpressionSemantics.GreaterThanOrEqual => new P("gte", value),
                ExpressionSemantics.LowerThanOrEqual => new P("lte", value),
                ExpressionSemantics.IsContainedIn => P.Within(value),
                _ => throw new ExpressionNotSupportedException()
            };
        }

        private static object[] SubStrings(string value)
        {
            var ret = new object[value.Length + 1];

            for(var i = 0; i < ret.Length; i++)
            {
                ret[i] = value.Substring(0, i);
            }

            return ret;
        }
    }
}
