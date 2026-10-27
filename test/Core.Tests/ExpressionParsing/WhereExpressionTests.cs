using ExRam.Gremlinq.Core.ExpressionParsing;

using FluentAssertions;

namespace ExRam.Gremlinq.Core.Tests
{
    public class WhereExpressionTests
    {
        [Fact]
        public void Equals_object_of_an_equal_where_expression() => WhereExpression.True
            .Equals((object)new WhereExpression(Expressions.True, EqualsExpressionSemantics.Instance, Expressions.True))
            .Should()
            .BeTrue();

        [Fact]
        public void Equals_object_of_another_type() => WhereExpression.True
            .Equals((object)true)
            .Should()
            .BeFalse();

        [Fact]
        public void Equal_where_expressions_have_equal_hash_codes() => WhereExpression.True
            .GetHashCode()
            .Should()
            .Be(new WhereExpression(Expressions.True, EqualsExpressionSemantics.Instance, Expressions.True).GetHashCode());
    }
}
