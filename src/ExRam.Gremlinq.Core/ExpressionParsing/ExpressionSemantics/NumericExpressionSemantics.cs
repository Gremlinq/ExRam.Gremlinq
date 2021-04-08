using System;

namespace ExRam.Gremlinq.Core
{
    internal sealed class NumericExpressionSemantics : ObjectExpressionSemantics
    {
        public static readonly NumericExpressionSemantics LowerThan = new(() => GreaterThan!);
        public static readonly NumericExpressionSemantics LowerThanOrEqual = new(() => GreaterThanOrEqual!);
        public static readonly NumericExpressionSemantics GreaterThanOrEqual = new(() => LowerThanOrEqual!);
        public static readonly NumericExpressionSemantics GreaterThan = new(() => LowerThan!);

        private NumericExpressionSemantics(Func<ExpressionSemantics> flip) : base(flip)
        {

        }
    }
}