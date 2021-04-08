using System;

namespace ExRam.Gremlinq.Core
{
    internal sealed class StringExpressionSemantics : ExpressionSemantics
    {
        public static readonly StringExpressionSemantics HasInfix = new(() => IsInfixOf!);
        public static readonly StringExpressionSemantics StartsWith = new(() => IsPrefixOf!);
        public static readonly StringExpressionSemantics EndsWith = new(() => IsSuffixOf!);

        public static readonly StringExpressionSemantics IsInfixOf = new(() => HasInfix!);
        public static readonly StringExpressionSemantics IsPrefixOf = new(() => StartsWith!);
        public static readonly StringExpressionSemantics IsSuffixOf = new(() => EndsWith!);

        private StringExpressionSemantics(Func<ExpressionSemantics> flip) : base(flip)
        {

        }
    }
}