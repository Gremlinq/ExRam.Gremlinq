using System.Linq.Expressions;

using FluentAssertions;

namespace ExRam.Gremlinq.Core.Tests
{
    public class ExpressionNotSupportedExceptionTests
    {
        [Fact]
        public void Default_constructor()
        {
            var ex = new ExpressionNotSupportedException();

            ex.Message.Should().Be("An expression is not supported.");
            ex.InnerException.Should().BeNull();
        }

        [Fact]
        public void Message_constructor()
        {
            var ex = new ExpressionNotSupportedException("custom message");

            ex.Message.Should().Be("custom message");
        }

        [Fact]
        public void Message_constructor_null_throws()
        {
            FluentActions.Invoking(() => new ExpressionNotSupportedException((string)null!))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Expression_constructor()
        {
            Expression<Func<int>> expr = () => 42;

            var ex = new ExpressionNotSupportedException(expr);

            ex.Message.Should().Contain("is not supported");
        }

        [Fact]
        public void Expression_constructor_null_throws()
        {
            FluentActions.Invoking(() => new ExpressionNotSupportedException((Expression)null!))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Expression_and_inner_constructor()
        {
            Expression<Func<int>> expr = () => 42;
            var inner = new InvalidOperationException("inner");

            var ex = new ExpressionNotSupportedException(expr, inner);

            ex.Message.Should().Contain("is not supported");
            ex.InnerException.Should().BeSameAs(inner);
        }

        [Fact]
        public void Expression_and_inner_constructor_null_expression_throws()
        {
            FluentActions.Invoking(() => new ExpressionNotSupportedException((Expression)null!, new Exception()))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Expression_and_inner_constructor_null_inner_throws()
        {
            Expression<Func<int>> expr = () => 42;

            FluentActions.Invoking(() => new ExpressionNotSupportedException(expr, null!))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void InnerException_constructor()
        {
            var inner = new InvalidOperationException("inner");

            var ex = new ExpressionNotSupportedException(inner);

            ex.Message.Should().Be("An expression is not supported.");
            ex.InnerException.Should().BeSameAs(inner);
        }

        [Fact]
        public void InnerException_constructor_null_throws()
        {
            FluentActions.Invoking(() => new ExpressionNotSupportedException((Exception)null!))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Unwrap_nested_ExpressionNotSupportedException_with_standard_message()
        {
            var deepInner = new InvalidOperationException("root cause");
            var wrappedOnce = new ExpressionNotSupportedException(deepInner);

            var ex = new ExpressionNotSupportedException(wrappedOnce);

            ex.InnerException.Should().BeSameAs(deepInner);
        }

        [Fact]
        public void Does_not_unwrap_ExpressionNotSupportedException_with_custom_message()
        {
            var inner = new ExpressionNotSupportedException("custom");

            var ex = new ExpressionNotSupportedException(inner);

            ex.InnerException.Should().BeSameAs(inner);
        }
    }
}
