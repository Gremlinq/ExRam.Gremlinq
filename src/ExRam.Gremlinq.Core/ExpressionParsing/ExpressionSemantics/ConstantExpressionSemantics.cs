using System;

namespace ExRam.Gremlinq.Core
{
    internal sealed class ConstantExpressionSemantics : ExpressionSemantics
    {
        public static readonly ExpressionSemantics False = new ConstantExpressionSemantics(() => False!);
        public static readonly ExpressionSemantics True = new ConstantExpressionSemantics(() => True!);

        private ConstantExpressionSemantics(Func<ExpressionSemantics> flip) : base(flip)
        {

        }
    }
}