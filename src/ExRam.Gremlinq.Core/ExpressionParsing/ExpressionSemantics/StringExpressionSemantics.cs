using System;

namespace ExRam.Gremlinq.Core
{
    internal sealed class HasInfixExpressionSemantics : StringExpressionSemantics
    {
        public static readonly HasInfixExpressionSemantics CaseSensitive = new(StringComparison.Ordinal);
        public static readonly HasInfixExpressionSemantics CaseInsensitive = new(StringComparison.OrdinalIgnoreCase);

        private HasInfixExpressionSemantics(StringComparison comparison) : base(comparison)
        {

        }

        public override ExpressionSemantics Flip() => Comparison switch
        {
            StringComparison.Ordinal => IsInfixOfExpressionSemantics.CaseSensitive,
            StringComparison.OrdinalIgnoreCase => IsInfixOfExpressionSemantics.CaseInsensitive,
            _ => throw new ExpressionNotSupportedException()
        };
    }

    internal sealed class StartsWithExpressionSemantics : StringExpressionSemantics
    {
        public static readonly StartsWithExpressionSemantics CaseSensitive = new(StringComparison.Ordinal);
        public static readonly StartsWithExpressionSemantics CaseInsensitive = new(StringComparison.OrdinalIgnoreCase);

        private StartsWithExpressionSemantics(StringComparison comparison) : base(comparison)
        {

        }

        public override ExpressionSemantics Flip() => Comparison switch
        {
            StringComparison.Ordinal => IsPrefixOfExpressionSemantics.CaseSensitive,
            StringComparison.OrdinalIgnoreCase => IsPrefixOfExpressionSemantics.CaseInsensitive,
            _ => throw new ExpressionNotSupportedException()
        };
    }

    internal sealed class EndsWithExpressionSemantics : StringExpressionSemantics
    {
        public static readonly EndsWithExpressionSemantics CaseSensitive = new(StringComparison.Ordinal);
        public static readonly EndsWithExpressionSemantics CaseInsensitive = new(StringComparison.OrdinalIgnoreCase);

        private EndsWithExpressionSemantics(StringComparison comparison) : base(comparison)
        {

        }

        public override ExpressionSemantics Flip() => Comparison switch
        {
            StringComparison.Ordinal => IsSuffixOfExpressionSemantics.CaseSensitive,
            StringComparison.OrdinalIgnoreCase => IsSuffixOfExpressionSemantics.CaseInsensitive,
            _ => throw new ExpressionNotSupportedException()
        };
    }

    internal sealed class IsInfixOfExpressionSemantics : StringExpressionSemantics
    {
        public static readonly IsInfixOfExpressionSemantics CaseSensitive = new(StringComparison.Ordinal);
        public static readonly IsInfixOfExpressionSemantics CaseInsensitive = new(StringComparison.OrdinalIgnoreCase);

        private IsInfixOfExpressionSemantics(StringComparison comparison) : base(comparison)
        {

        }

        public override ExpressionSemantics Flip() => Comparison switch
        {
            StringComparison.Ordinal => HasInfixExpressionSemantics.CaseSensitive,
            StringComparison.OrdinalIgnoreCase => HasInfixExpressionSemantics.CaseInsensitive,
            _ => throw new ExpressionNotSupportedException()
        };
    }

    internal sealed class IsPrefixOfExpressionSemantics : StringExpressionSemantics
    {
        public static readonly IsPrefixOfExpressionSemantics CaseSensitive = new(StringComparison.Ordinal);
        public static readonly IsPrefixOfExpressionSemantics CaseInsensitive = new(StringComparison.OrdinalIgnoreCase);

        private IsPrefixOfExpressionSemantics(StringComparison comparison) : base(comparison)
        {

        }

        public override ExpressionSemantics Flip() => Comparison switch
        {
            StringComparison.Ordinal => StartsWithExpressionSemantics.CaseSensitive,
            StringComparison.OrdinalIgnoreCase => StartsWithExpressionSemantics.CaseInsensitive,
            _ => throw new ExpressionNotSupportedException()
        };
    }

    internal sealed class IsSuffixOfExpressionSemantics : StringExpressionSemantics
    {
        public static readonly IsSuffixOfExpressionSemantics CaseSensitive = new(StringComparison.Ordinal);
        public static readonly IsSuffixOfExpressionSemantics CaseInsensitive = new(StringComparison.OrdinalIgnoreCase);

        private IsSuffixOfExpressionSemantics(StringComparison comparison) : base(comparison)
        {

        }

        public override ExpressionSemantics Flip() => Comparison switch
        {
            StringComparison.Ordinal => EndsWithExpressionSemantics.CaseSensitive,
            StringComparison.OrdinalIgnoreCase => EndsWithExpressionSemantics.CaseInsensitive,
            _ => throw new ExpressionNotSupportedException()
        };
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
