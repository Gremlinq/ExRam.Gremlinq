using ExRam.Gremlinq.Core.ExpressionParsing;

using FluentAssertions;

namespace ExRam.Gremlinq.Core.Tests
{
    public class ExpressionSemanticsTests
    {
        [Fact]
        public void Equals_Flip_returns_self()
        {
            EqualsExpressionSemantics.Instance.Flip()
                .Should().BeSameAs(EqualsExpressionSemantics.Instance);
        }

        [Fact]
        public void NotEquals_Flip_returns_self()
        {
            NotEqualsExpressionSemantics.Instance.Flip()
                .Should().BeSameAs(NotEqualsExpressionSemantics.Instance);
        }

        [Fact]
        public void LowerThan_Flip_returns_GreaterThan()
        {
            LowerThanExpressionSemantics.Instance.Flip()
                .Should().BeSameAs(GreaterThanExpressionSemantics.Instance);
        }

        [Fact]
        public void GreaterThan_Flip_returns_LowerThan()
        {
            GreaterThanExpressionSemantics.Instance.Flip()
                .Should().BeSameAs(LowerThanExpressionSemantics.Instance);
        }

        [Fact]
        public void LowerThanOrEqual_Flip_returns_GreaterThanOrEqual()
        {
            LowerThanOrEqualExpressionSemantics.Instance.Flip()
                .Should().BeSameAs(GreaterThanOrEqualExpressionSemantics.Instance);
        }

        [Fact]
        public void GreaterThanOrEqual_Flip_returns_LowerThanOrEqual()
        {
            GreaterThanOrEqualExpressionSemantics.Instance.Flip()
                .Should().BeSameAs(LowerThanOrEqualExpressionSemantics.Instance);
        }

        [Fact]
        public void Contains_Flip_returns_IsContainedIn()
        {
            ContainsExpressionSemantics.Instance.Flip()
                .Should().BeSameAs(IsContainedInExpressionSemantics.Instance);
        }

        [Fact]
        public void IsContainedIn_Flip_returns_Contains()
        {
            IsContainedInExpressionSemantics.Instance.Flip()
                .Should().BeSameAs(ContainsExpressionSemantics.Instance);
        }

        [Fact]
        public void Intersects_Flip_returns_self()
        {
            IntersectsExpressionSemantics.Instance.Flip()
                .Should().BeSameAs(IntersectsExpressionSemantics.Instance);
        }

        [Fact]
        public void True_Flip_returns_self()
        {
            TrueExpressionSemantics.Instance.Flip()
                .Should().BeSameAs(TrueExpressionSemantics.Instance);
        }

        [Fact]
        public void False_Flip_returns_self()
        {
            FalseExpressionSemantics.Instance.Flip()
                .Should().BeSameAs(FalseExpressionSemantics.Instance);
        }

        [Fact]
        public void HasInfix_Flip_returns_IsInfixOf_CaseSensitive()
        {
            HasInfixExpressionSemantics.CaseSensitive.Flip()
                .Should().BeSameAs(IsInfixOfExpressionSemantics.CaseSensitive);
        }

        [Fact]
        public void HasInfix_Flip_returns_IsInfixOf_CaseInsensitive()
        {
            HasInfixExpressionSemantics.CaseInsensitive.Flip()
                .Should().BeSameAs(IsInfixOfExpressionSemantics.CaseInsensitive);
        }

        [Fact]
        public void IsInfixOf_Flip_returns_HasInfix()
        {
            IsInfixOfExpressionSemantics.CaseSensitive.Flip()
                .Should().BeSameAs(HasInfixExpressionSemantics.CaseSensitive);
        }

        [Fact]
        public void StartsWith_Flip_returns_IsPrefixOf()
        {
            StartsWithExpressionSemantics.CaseSensitive.Flip()
                .Should().BeSameAs(IsPrefixOfExpressionSemantics.CaseSensitive);
        }

        [Fact]
        public void IsPrefixOf_Flip_returns_StartsWith()
        {
            IsPrefixOfExpressionSemantics.CaseInsensitive.Flip()
                .Should().BeSameAs(StartsWithExpressionSemantics.CaseInsensitive);
        }

        [Fact]
        public void EndsWith_Flip_returns_IsSuffixOf()
        {
            EndsWithExpressionSemantics.CaseSensitive.Flip()
                .Should().BeSameAs(IsSuffixOfExpressionSemantics.CaseSensitive);
        }

        [Fact]
        public void IsSuffixOf_Flip_returns_EndsWith()
        {
            IsSuffixOfExpressionSemantics.CaseInsensitive.Flip()
                .Should().BeSameAs(EndsWithExpressionSemantics.CaseInsensitive);
        }

        [Fact]
        public void StringEquals_Flip_returns_self()
        {
            StringEqualsExpressionSemantics.CaseSensitive.Flip()
                .Should().BeSameAs(StringEqualsExpressionSemantics.CaseSensitive);
        }

        [Fact]
        public void StringSemantics_Comparison_property()
        {
            HasInfixExpressionSemantics.CaseSensitive.Comparison
                .Should().Be(StringComparison.Ordinal);

            HasInfixExpressionSemantics.CaseInsensitive.Comparison
                .Should().Be(StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void StringEquals_Get_unsupported_comparison_throws()
        {
            FluentActions.Invoking(() => StringEqualsExpressionSemantics.Get(StringComparison.CurrentCulture))
                .Should().Throw<ExpressionNotSupportedException>();
        }

        [Fact]
        public void HasInfix_Get_unsupported_comparison_throws()
        {
            FluentActions.Invoking(() => HasInfixExpressionSemantics.Get(StringComparison.CurrentCulture))
                .Should().Throw<ExpressionNotSupportedException>();
        }

        [Fact]
        public void StartsWith_Get_unsupported_comparison_throws()
        {
            FluentActions.Invoking(() => StartsWithExpressionSemantics.Get(StringComparison.InvariantCulture))
                .Should().Throw<ExpressionNotSupportedException>();
        }

        [Fact]
        public void EndsWith_Get_unsupported_comparison_throws()
        {
            FluentActions.Invoking(() => EndsWithExpressionSemantics.Get(StringComparison.InvariantCultureIgnoreCase))
                .Should().Throw<ExpressionNotSupportedException>();
        }

        [Fact]
        public void IsInfixOf_Get_unsupported_comparison_throws()
        {
            FluentActions.Invoking(() => IsInfixOfExpressionSemantics.Get(StringComparison.CurrentCulture))
                .Should().Throw<ExpressionNotSupportedException>();
        }

        [Fact]
        public void IsPrefixOf_Get_unsupported_comparison_throws()
        {
            FluentActions.Invoking(() => IsPrefixOfExpressionSemantics.Get(StringComparison.CurrentCulture))
                .Should().Throw<ExpressionNotSupportedException>();
        }

        [Fact]
        public void IsSuffixOf_Get_unsupported_comparison_throws()
        {
            FluentActions.Invoking(() => IsSuffixOfExpressionSemantics.Get(StringComparison.CurrentCulture))
                .Should().Throw<ExpressionNotSupportedException>();
        }
    }
}
