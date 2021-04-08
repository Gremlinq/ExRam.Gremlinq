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

    internal sealed class EnumerableExpressionSemantics : ExpressionSemantics
    {
        public static readonly EnumerableExpressionSemantics Intersects = new(() => Intersects!);
        public static readonly EnumerableExpressionSemantics Contains = new(() => IsContainedIn!);
        public static readonly EnumerableExpressionSemantics IsContainedIn = new(() => Contains!);

        private EnumerableExpressionSemantics(Func<ExpressionSemantics> flip) : base(flip)
        {

        }
    }

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

    internal abstract class ExpressionSemantics
    {
        private readonly Func<ExpressionSemantics> _flip;

        protected ExpressionSemantics(Func<ExpressionSemantics> flip)
        {
            _flip = flip;
        }

        public ExpressionSemantics Flip() => _flip();
    }
}
