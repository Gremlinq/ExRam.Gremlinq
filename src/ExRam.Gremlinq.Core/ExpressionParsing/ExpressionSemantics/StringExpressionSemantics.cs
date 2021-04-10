using System;

namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    internal sealed class StringEqualsExpressionSemantics : StringExpressionSemantics
    {
        public static readonly StringEqualsExpressionSemantics CaseSensitive = new(StringComparison.Ordinal);
        public static readonly StringEqualsExpressionSemantics CaseInsensitive = new(StringComparison.OrdinalIgnoreCase);

        private StringEqualsExpressionSemantics(StringComparison comparison) : base(comparison)
        {

        }

        public override ExpressionSemantics Flip() => Get(Comparison);

        public static StringEqualsExpressionSemantics Get(StringComparison comparison)
        {
            return comparison switch
            {
                StringComparison.Ordinal => CaseSensitive,
                StringComparison.OrdinalIgnoreCase => CaseInsensitive,
                _ => throw new ExpressionNotSupportedException()
            };
        }
    }

    internal sealed class HasInfixExpressionSemantics : StringExpressionSemantics
    {
        public static readonly HasInfixExpressionSemantics CaseSensitive = new(StringComparison.Ordinal);
        public static readonly HasInfixExpressionSemantics CaseInsensitive = new(StringComparison.OrdinalIgnoreCase);

        private HasInfixExpressionSemantics(StringComparison comparison) : base(comparison)
        {

        }

        public override ExpressionSemantics Flip() => IsInfixOfExpressionSemantics.Get(Comparison);

        public static HasInfixExpressionSemantics Get(StringComparison comparison)
        {
            return comparison switch
            {
                StringComparison.Ordinal => CaseSensitive,
                StringComparison.OrdinalIgnoreCase => CaseInsensitive,
                _ => throw new ExpressionNotSupportedException()
            };
        }
    }

    internal sealed class StartsWithExpressionSemantics : StringExpressionSemantics
    {
        public static readonly StartsWithExpressionSemantics CaseSensitive = new(StringComparison.Ordinal);
        public static readonly StartsWithExpressionSemantics CaseInsensitive = new(StringComparison.OrdinalIgnoreCase);

        private StartsWithExpressionSemantics(StringComparison comparison) : base(comparison)
        {

        }

        public override ExpressionSemantics Flip() => IsPrefixOfExpressionSemantics.Get(Comparison);

        public static StartsWithExpressionSemantics Get(StringComparison comparison)
        {
            return comparison switch
            {
                StringComparison.Ordinal => CaseSensitive,
                StringComparison.OrdinalIgnoreCase => CaseInsensitive,
                _ => throw new ExpressionNotSupportedException()
            };
        }
    }

    internal sealed class EndsWithExpressionSemantics : StringExpressionSemantics
    {
        public static readonly EndsWithExpressionSemantics CaseSensitive = new(StringComparison.Ordinal);
        public static readonly EndsWithExpressionSemantics CaseInsensitive = new(StringComparison.OrdinalIgnoreCase);

        private EndsWithExpressionSemantics(StringComparison comparison) : base(comparison)
        {

        }

        public override ExpressionSemantics Flip() => IsSuffixOfExpressionSemantics.Get(Comparison);

        public static EndsWithExpressionSemantics Get(StringComparison comparison)
        {
            return comparison switch
            {
                StringComparison.Ordinal => CaseSensitive,
                StringComparison.OrdinalIgnoreCase => CaseInsensitive,
                _ => throw new ExpressionNotSupportedException()
            };
        }
    }

    internal sealed class IsInfixOfExpressionSemantics : StringExpressionSemantics
    {
        public static readonly IsInfixOfExpressionSemantics CaseSensitive = new(StringComparison.Ordinal);
        public static readonly IsInfixOfExpressionSemantics CaseInsensitive = new(StringComparison.OrdinalIgnoreCase);

        private IsInfixOfExpressionSemantics(StringComparison comparison) : base(comparison)
        {

        }

        public override ExpressionSemantics Flip() => HasInfixExpressionSemantics.Get(Comparison);

        public static IsInfixOfExpressionSemantics Get(StringComparison comparison)
        {
            return comparison switch
            {
                StringComparison.Ordinal => CaseSensitive,
                StringComparison.OrdinalIgnoreCase => CaseInsensitive,
                _ => throw new ExpressionNotSupportedException()
            };
        }
    }

    internal sealed class IsPrefixOfExpressionSemantics : StringExpressionSemantics
    {
        public static readonly IsPrefixOfExpressionSemantics CaseSensitive = new(StringComparison.Ordinal);
        public static readonly IsPrefixOfExpressionSemantics CaseInsensitive = new(StringComparison.OrdinalIgnoreCase);

        private IsPrefixOfExpressionSemantics(StringComparison comparison) : base(comparison)
        {

        }

        public override ExpressionSemantics Flip() => StartsWithExpressionSemantics.Get(Comparison);

        public static IsPrefixOfExpressionSemantics Get(StringComparison comparison)
        {
            return comparison switch
            {
                StringComparison.Ordinal => CaseSensitive,
                StringComparison.OrdinalIgnoreCase => CaseInsensitive,
                _ => throw new ExpressionNotSupportedException()
            };
        }
    }

    internal sealed class IsSuffixOfExpressionSemantics : StringExpressionSemantics
    {
        public static readonly IsSuffixOfExpressionSemantics CaseSensitive = new(StringComparison.Ordinal);
        public static readonly IsSuffixOfExpressionSemantics CaseInsensitive = new(StringComparison.OrdinalIgnoreCase);

        private IsSuffixOfExpressionSemantics(StringComparison comparison) : base(comparison)
        {

        }

        public override ExpressionSemantics Flip() => EndsWithExpressionSemantics.Get(Comparison);

        public static IsSuffixOfExpressionSemantics Get(StringComparison comparison)
        {
            return comparison switch
            {
                StringComparison.Ordinal => CaseSensitive,
                StringComparison.OrdinalIgnoreCase => CaseInsensitive,
                _ => throw new ExpressionNotSupportedException()
            };
        }
    }
    
    internal abstract class StringExpressionSemantics : ExpressionSemantics
    {
        protected StringExpressionSemantics(StringComparison comparison)
        {
            Comparison = comparison;
        }

        public StringComparison Comparison { get; }
    }
}
