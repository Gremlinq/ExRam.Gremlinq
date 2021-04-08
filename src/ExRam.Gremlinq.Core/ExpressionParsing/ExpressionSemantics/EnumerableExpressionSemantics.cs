using System;

namespace ExRam.Gremlinq.Core
{
    internal sealed class EnumerableExpressionSemantics : ExpressionSemantics
    {
        public static readonly EnumerableExpressionSemantics Intersects = new(() => Intersects!);
        public static readonly EnumerableExpressionSemantics Contains = new(() => IsContainedIn!);
        public static readonly EnumerableExpressionSemantics IsContainedIn = new(() => Contains!);

        private EnumerableExpressionSemantics(Func<ExpressionSemantics> flip) : base(flip)
        {

        }
    }
}