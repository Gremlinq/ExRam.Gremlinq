namespace ExRam.Gremlinq.Core.ExpressionParsing
{
    /// <summary>Represents string equality comparison semantics with configurable case sensitivity.</summary>
    public sealed class StringEqualsExpressionSemantics : StringExpressionSemantics
    {
        /// <summary>Gets the case-sensitive instance.</summary>
        public static readonly StringEqualsExpressionSemantics CaseSensitive = new(StringComparison.Ordinal);
        /// <summary>Gets the case-insensitive instance.</summary>
        public static readonly StringEqualsExpressionSemantics CaseInsensitive = new(StringComparison.OrdinalIgnoreCase);

        private StringEqualsExpressionSemantics(StringComparison comparison) : base(comparison)
        {

        }

        /// <inheritdoc />
        public override ExpressionSemantics Flip() => Get(Comparison);

        /// <summary>Gets the instance for the specified string comparison.</summary>
        /// <param name="comparison">The string comparison mode.</param>
        public static StringEqualsExpressionSemantics Get(StringComparison comparison) => comparison switch
        {
            StringComparison.Ordinal => CaseSensitive,
            StringComparison.OrdinalIgnoreCase => CaseInsensitive,
            _ => throw new ExpressionNotSupportedException()
        };
    }

    /// <summary>Represents "has infix" (contains substring) semantics, translating to a text predicate.</summary>
    public sealed class HasInfixExpressionSemantics : StringExpressionSemantics
    {
        /// <summary>Gets the case-sensitive instance.</summary>
        public static readonly HasInfixExpressionSemantics CaseSensitive = new(StringComparison.Ordinal);
        /// <summary>Gets the case-insensitive instance.</summary>
        public static readonly HasInfixExpressionSemantics CaseInsensitive = new(StringComparison.OrdinalIgnoreCase);

        private HasInfixExpressionSemantics(StringComparison comparison) : base(comparison)
        {

        }

        /// <inheritdoc />
        public override ExpressionSemantics Flip() => IsInfixOfExpressionSemantics.Get(Comparison);

        /// <summary>Gets the instance for the specified string comparison.</summary>
        /// <param name="comparison">The string comparison mode.</param>
        public static HasInfixExpressionSemantics Get(StringComparison comparison) => comparison switch
        {
            StringComparison.Ordinal => CaseSensitive,
            StringComparison.OrdinalIgnoreCase => CaseInsensitive,
            _ => throw new ExpressionNotSupportedException()
        };
    }

    /// <summary>Represents "starts with" semantics, translating to a text predicate.</summary>
    public sealed class StartsWithExpressionSemantics : StringExpressionSemantics
    {
        /// <summary>Gets the case-sensitive instance.</summary>
        public static readonly StartsWithExpressionSemantics CaseSensitive = new(StringComparison.Ordinal);
        /// <summary>Gets the case-insensitive instance.</summary>
        public static readonly StartsWithExpressionSemantics CaseInsensitive = new(StringComparison.OrdinalIgnoreCase);

        private StartsWithExpressionSemantics(StringComparison comparison) : base(comparison)
        {

        }

        /// <inheritdoc />
        public override ExpressionSemantics Flip() => IsPrefixOfExpressionSemantics.Get(Comparison);

        /// <summary>Gets the instance for the specified string comparison.</summary>
        /// <param name="comparison">The string comparison mode.</param>
        public static StartsWithExpressionSemantics Get(StringComparison comparison) => comparison switch
        {
            StringComparison.Ordinal => CaseSensitive,
            StringComparison.OrdinalIgnoreCase => CaseInsensitive,
            _ => throw new ExpressionNotSupportedException()
        };
    }

    /// <summary>Represents "ends with" semantics, translating to a text predicate.</summary>
    public sealed class EndsWithExpressionSemantics : StringExpressionSemantics
    {
        /// <summary>Gets the case-sensitive instance.</summary>
        public static readonly EndsWithExpressionSemantics CaseSensitive = new(StringComparison.Ordinal);
        /// <summary>Gets the case-insensitive instance.</summary>
        public static readonly EndsWithExpressionSemantics CaseInsensitive = new(StringComparison.OrdinalIgnoreCase);

        private EndsWithExpressionSemantics(StringComparison comparison) : base(comparison)
        {

        }

        /// <inheritdoc />
        public override ExpressionSemantics Flip() => IsSuffixOfExpressionSemantics.Get(Comparison);

        /// <summary>Gets the instance for the specified string comparison.</summary>
        /// <param name="comparison">The string comparison mode.</param>
        public static EndsWithExpressionSemantics Get(StringComparison comparison) => comparison switch
        {
            StringComparison.Ordinal => CaseSensitive,
            StringComparison.OrdinalIgnoreCase => CaseInsensitive,
            _ => throw new ExpressionNotSupportedException()
        };
    }

    /// <summary>Represents "is infix of" semantics, the flipped form of <see cref="HasInfixExpressionSemantics"/>.</summary>
    public sealed class IsInfixOfExpressionSemantics : StringExpressionSemantics
    {
        /// <summary>Gets the case-sensitive instance.</summary>
        public static readonly IsInfixOfExpressionSemantics CaseSensitive = new(StringComparison.Ordinal);
        /// <summary>Gets the case-insensitive instance.</summary>
        public static readonly IsInfixOfExpressionSemantics CaseInsensitive = new(StringComparison.OrdinalIgnoreCase);

        private IsInfixOfExpressionSemantics(StringComparison comparison) : base(comparison)
        {

        }

        /// <inheritdoc />
        public override ExpressionSemantics Flip() => HasInfixExpressionSemantics.Get(Comparison);

        /// <summary>Gets the instance for the specified string comparison.</summary>
        /// <param name="comparison">The string comparison mode.</param>
        public static IsInfixOfExpressionSemantics Get(StringComparison comparison) => comparison switch
        {
            StringComparison.Ordinal => CaseSensitive,
            StringComparison.OrdinalIgnoreCase => CaseInsensitive,
            _ => throw new ExpressionNotSupportedException()
        };
    }

    /// <summary>Represents "is prefix of" semantics, the flipped form of <see cref="StartsWithExpressionSemantics"/>.</summary>
    public sealed class IsPrefixOfExpressionSemantics : StringExpressionSemantics
    {
        /// <summary>Gets the case-sensitive instance.</summary>
        public static readonly IsPrefixOfExpressionSemantics CaseSensitive = new(StringComparison.Ordinal);
        /// <summary>Gets the case-insensitive instance.</summary>
        public static readonly IsPrefixOfExpressionSemantics CaseInsensitive = new(StringComparison.OrdinalIgnoreCase);

        private IsPrefixOfExpressionSemantics(StringComparison comparison) : base(comparison)
        {

        }

        /// <inheritdoc />
        public override ExpressionSemantics Flip() => StartsWithExpressionSemantics.Get(Comparison);

        /// <summary>Gets the instance for the specified string comparison.</summary>
        /// <param name="comparison">The string comparison mode.</param>
        public static IsPrefixOfExpressionSemantics Get(StringComparison comparison) => comparison switch
        {
            StringComparison.Ordinal => CaseSensitive,
            StringComparison.OrdinalIgnoreCase => CaseInsensitive,
            _ => throw new ExpressionNotSupportedException()
        };
    }

    /// <summary>Represents "is suffix of" semantics, the flipped form of <see cref="EndsWithExpressionSemantics"/>.</summary>
    public sealed class IsSuffixOfExpressionSemantics : StringExpressionSemantics
    {
        /// <summary>Gets the case-sensitive instance.</summary>
        public static readonly IsSuffixOfExpressionSemantics CaseSensitive = new(StringComparison.Ordinal);
        /// <summary>Gets the case-insensitive instance.</summary>
        public static readonly IsSuffixOfExpressionSemantics CaseInsensitive = new(StringComparison.OrdinalIgnoreCase);

        private IsSuffixOfExpressionSemantics(StringComparison comparison) : base(comparison)
        {

        }

        /// <inheritdoc />
        public override ExpressionSemantics Flip() => EndsWithExpressionSemantics.Get(Comparison);

        /// <summary>Gets the instance for the specified string comparison.</summary>
        /// <param name="comparison">The string comparison mode.</param>
        public static IsSuffixOfExpressionSemantics Get(StringComparison comparison) => comparison switch
        {
            StringComparison.Ordinal => CaseSensitive,
            StringComparison.OrdinalIgnoreCase => CaseInsensitive,
            _ => throw new ExpressionNotSupportedException()
        };
    }

    /// <summary>Base class for expression semantics operating on string comparisons with configurable case sensitivity.</summary>
    public abstract class StringExpressionSemantics : ExpressionSemantics
    {
        /// <summary>Initializes a new instance of <see cref="StringExpressionSemantics"/>.</summary>
        /// <param name="comparison">The string comparison mode.</param>
        protected StringExpressionSemantics(StringComparison comparison)
        {
            Comparison = comparison;
        }

        /// <summary>Gets the string comparison mode.</summary>
        public StringComparison Comparison { get; }
    }
}
