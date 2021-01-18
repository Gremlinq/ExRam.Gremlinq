using System;

namespace ExRam.Gremlinq.Core
{
    internal static class ExpressionFragmentExtensions
    {
        public static object? GetValue(this ExpressionFragment expressionFragment)
        {
            return expressionFragment switch
            {
                ConstantExpressionFragment c => c.Value,
                { } x => x.Expression?.GetValue(),
                _ => throw new ArgumentException()
            };
        }
    }
}
