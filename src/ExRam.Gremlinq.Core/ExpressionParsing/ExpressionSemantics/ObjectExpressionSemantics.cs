using System;

namespace ExRam.Gremlinq.Core
{
    internal abstract class ObjectExpressionSemantics : ExpressionSemantics
    {
        private sealed class ObjectExpressionSemanticsImpl : ObjectExpressionSemantics
        {
            public ObjectExpressionSemanticsImpl(Func<ExpressionSemantics> flip) : base(flip)
            {

            }
        }

        public static new readonly ObjectExpressionSemantics Equals = new ObjectExpressionSemanticsImpl(() => Equals!);
        public static readonly ObjectExpressionSemantics NotEquals = new ObjectExpressionSemanticsImpl(() => NotEquals!);

        protected ObjectExpressionSemantics(Func<ExpressionSemantics> flip) : base(flip)
        {
        }

        public ExpressionSemantics TransformCompareTo(int comparison)
        {
            if (this == NumericExpressionSemantics.LowerThan)
            {
                return comparison switch
                {
                    0 => NumericExpressionSemantics.LowerThan,
                    1 => NumericExpressionSemantics.LowerThanOrEqual,
                    > 1 => ConstantExpressionSemantics.True,
                    _ => ConstantExpressionSemantics.False
                };
            }

            if (this == NumericExpressionSemantics.LowerThanOrEqual)
            {
                return comparison switch
                {
                    -1 => NumericExpressionSemantics.LowerThan,
                    0 => NumericExpressionSemantics.LowerThanOrEqual,
                    > 0 => ConstantExpressionSemantics.True,
                    _ => ConstantExpressionSemantics.False
                };
            }

            if (this == Equals)
            {
                return comparison switch
                {
                    -1 => NumericExpressionSemantics.LowerThan,
                    0 => Equals,
                    1 => NumericExpressionSemantics.GreaterThan,
                    _ => ConstantExpressionSemantics.False
                };
            }

            if (this == NumericExpressionSemantics.GreaterThanOrEqual)
            {
                return comparison switch
                {
                    <= -1 => ConstantExpressionSemantics.True,
                    0 => NumericExpressionSemantics.GreaterThanOrEqual,
                    1 => NumericExpressionSemantics.GreaterThan,
                    _ => ConstantExpressionSemantics.False
                };
            }

            if (this == NumericExpressionSemantics.GreaterThan)
            {
                return comparison switch
                {
                    < -1 => ConstantExpressionSemantics.True,
                    -1 => NumericExpressionSemantics.GreaterThanOrEqual,
                    0 => NumericExpressionSemantics.GreaterThan,
                    _ => ConstantExpressionSemantics.False
                };
            }

            if (this == NotEquals)
            {
                return comparison switch
                {
                    -1 => NumericExpressionSemantics.GreaterThanOrEqual,
                    0 => NotEquals,
                    1 => NumericExpressionSemantics.LowerThanOrEqual,
                    _ => ConstantExpressionSemantics.True
                };
            }

            throw new ArgumentException();
        }
    }
}