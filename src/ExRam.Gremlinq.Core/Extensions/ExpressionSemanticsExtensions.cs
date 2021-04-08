using System;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    internal static class ExpressionSemanticsExtensions
    {
        private static readonly P PNeqNull = P.Neq(new object?[] { null });
        
        public static P ToP(this ExpressionSemantics semantics, object? value)
        {
            if (semantics == EnumerableExpressionSemantics.Contains)
                return new P("eq", value);

            if (semantics == EnumerableExpressionSemantics.Intersects)
                return P.Within(value);

            if (semantics == EnumerableExpressionSemantics.IsContainedIn)
                return P.Within(value);

            if (semantics is StringExpressionSemantics && value is string stringValue)
            {
                if (semantics == StringExpressionSemantics.IsPrefixOf)
                    return P.Within(SubStrings(stringValue));

                if (semantics == StringExpressionSemantics.HasInfix)
                {
                    return stringValue.Length > 0
                        ? TextP.Containing(stringValue)
                        : PNeqNull;
                }

                if (semantics == StringExpressionSemantics.StartsWith)
                {
                    return stringValue.Length > 0
                        ? TextP.StartingWith(stringValue)
                        : PNeqNull;
                }

                if (semantics == StringExpressionSemantics.EndsWith)
                {
                    return stringValue.Length > 0
                        ? TextP.EndingWith(stringValue)
                        : PNeqNull;
                }
            }
            else if (semantics is ObjectExpressionSemantics)
            {
                if (semantics == ObjectExpressionSemantics.Equals)
                    return new P("eq", value);

                if (semantics == ObjectExpressionSemantics.NotEquals)
                    return new P("neq", value);

                if (semantics is NumericExpressionSemantics)
                {
                    if (semantics == NumericExpressionSemantics.LowerThan)
                        return new P("lt", value);

                    if (semantics == NumericExpressionSemantics.GreaterThan)
                        return new P("gt", value);

                    if (semantics == NumericExpressionSemantics.GreaterThanOrEqual)
                        return new P("gte", value);

                    if (semantics == NumericExpressionSemantics.LowerThanOrEqual)
                        return new P("lte", value);
                }
            }

            throw new ExpressionNotSupportedException();
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
