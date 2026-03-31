using ExRam.Gremlinq.Core.ExpressionParsing;

using FluentAssertions;

namespace ExRam.Gremlinq.Core.Tests
{
    public class TransformCompareToTests
    {
        [Fact]
        public void LowerThan_compareTo_0_returns_LowerThan()
        {
            LowerThanExpressionSemantics.Instance.TransformCompareTo(0)
                .Should().BeSameAs(LowerThanExpressionSemantics.Instance);
        }

        [Fact]
        public void LowerThan_compareTo_1_returns_LowerThanOrEqual()
        {
            LowerThanExpressionSemantics.Instance.TransformCompareTo(1)
                .Should().BeSameAs(LowerThanOrEqualExpressionSemantics.Instance);
        }

        [Fact]
        public void LowerThan_compareTo_2_returns_True()
        {
            LowerThanExpressionSemantics.Instance.TransformCompareTo(2)
                .Should().BeSameAs(TrueExpressionSemantics.Instance);
        }

        [Fact]
        public void LowerThan_compareTo_neg1_returns_False()
        {
            LowerThanExpressionSemantics.Instance.TransformCompareTo(-1)
                .Should().BeSameAs(FalseExpressionSemantics.Instance);
        }

        [Fact]
        public void LowerThanOrEqual_compareTo_neg1_returns_LowerThan()
        {
            LowerThanOrEqualExpressionSemantics.Instance.TransformCompareTo(-1)
                .Should().BeSameAs(LowerThanExpressionSemantics.Instance);
        }

        [Fact]
        public void LowerThanOrEqual_compareTo_0_returns_LowerThanOrEqual()
        {
            LowerThanOrEqualExpressionSemantics.Instance.TransformCompareTo(0)
                .Should().BeSameAs(LowerThanOrEqualExpressionSemantics.Instance);
        }

        [Fact]
        public void LowerThanOrEqual_compareTo_1_returns_True()
        {
            LowerThanOrEqualExpressionSemantics.Instance.TransformCompareTo(1)
                .Should().BeSameAs(TrueExpressionSemantics.Instance);
        }

        [Fact]
        public void LowerThanOrEqual_compareTo_neg2_returns_False()
        {
            LowerThanOrEqualExpressionSemantics.Instance.TransformCompareTo(-2)
                .Should().BeSameAs(FalseExpressionSemantics.Instance);
        }

        [Fact]
        public void Equals_compareTo_neg1_returns_LowerThan()
        {
            EqualsExpressionSemantics.Instance.TransformCompareTo(-1)
                .Should().BeSameAs(LowerThanExpressionSemantics.Instance);
        }

        [Fact]
        public void Equals_compareTo_0_returns_Equals()
        {
            EqualsExpressionSemantics.Instance.TransformCompareTo(0)
                .Should().BeSameAs(EqualsExpressionSemantics.Instance);
        }

        [Fact]
        public void Equals_compareTo_1_returns_GreaterThan()
        {
            EqualsExpressionSemantics.Instance.TransformCompareTo(1)
                .Should().BeSameAs(GreaterThanExpressionSemantics.Instance);
        }

        [Fact]
        public void Equals_compareTo_2_returns_False()
        {
            EqualsExpressionSemantics.Instance.TransformCompareTo(2)
                .Should().BeSameAs(FalseExpressionSemantics.Instance);
        }

        [Fact]
        public void GreaterThanOrEqual_compareTo_neg1_returns_True()
        {
            GreaterThanOrEqualExpressionSemantics.Instance.TransformCompareTo(-1)
                .Should().BeSameAs(TrueExpressionSemantics.Instance);
        }

        [Fact]
        public void GreaterThanOrEqual_compareTo_0_returns_GreaterThanOrEqual()
        {
            GreaterThanOrEqualExpressionSemantics.Instance.TransformCompareTo(0)
                .Should().BeSameAs(GreaterThanOrEqualExpressionSemantics.Instance);
        }

        [Fact]
        public void GreaterThanOrEqual_compareTo_1_returns_GreaterThan()
        {
            GreaterThanOrEqualExpressionSemantics.Instance.TransformCompareTo(1)
                .Should().BeSameAs(GreaterThanExpressionSemantics.Instance);
        }

        [Fact]
        public void GreaterThanOrEqual_compareTo_2_returns_False()
        {
            GreaterThanOrEqualExpressionSemantics.Instance.TransformCompareTo(2)
                .Should().BeSameAs(FalseExpressionSemantics.Instance);
        }

        [Fact]
        public void GreaterThan_compareTo_neg2_returns_True()
        {
            GreaterThanExpressionSemantics.Instance.TransformCompareTo(-2)
                .Should().BeSameAs(TrueExpressionSemantics.Instance);
        }

        [Fact]
        public void GreaterThan_compareTo_neg1_returns_GreaterThanOrEqual()
        {
            GreaterThanExpressionSemantics.Instance.TransformCompareTo(-1)
                .Should().BeSameAs(GreaterThanOrEqualExpressionSemantics.Instance);
        }

        [Fact]
        public void GreaterThan_compareTo_0_returns_GreaterThan()
        {
            GreaterThanExpressionSemantics.Instance.TransformCompareTo(0)
                .Should().BeSameAs(GreaterThanExpressionSemantics.Instance);
        }

        [Fact]
        public void GreaterThan_compareTo_1_returns_False()
        {
            GreaterThanExpressionSemantics.Instance.TransformCompareTo(1)
                .Should().BeSameAs(FalseExpressionSemantics.Instance);
        }

        [Fact]
        public void NotEquals_compareTo_neg1_returns_GreaterThanOrEqual()
        {
            NotEqualsExpressionSemantics.Instance.TransformCompareTo(-1)
                .Should().BeSameAs(GreaterThanOrEqualExpressionSemantics.Instance);
        }

        [Fact]
        public void NotEquals_compareTo_0_returns_NotEquals()
        {
            NotEqualsExpressionSemantics.Instance.TransformCompareTo(0)
                .Should().BeSameAs(NotEqualsExpressionSemantics.Instance);
        }

        [Fact]
        public void NotEquals_compareTo_1_returns_LowerThanOrEqual()
        {
            NotEqualsExpressionSemantics.Instance.TransformCompareTo(1)
                .Should().BeSameAs(LowerThanOrEqualExpressionSemantics.Instance);
        }

        [Fact]
        public void NotEquals_compareTo_2_returns_True()
        {
            NotEqualsExpressionSemantics.Instance.TransformCompareTo(2)
                .Should().BeSameAs(TrueExpressionSemantics.Instance);
        }
    }
}
