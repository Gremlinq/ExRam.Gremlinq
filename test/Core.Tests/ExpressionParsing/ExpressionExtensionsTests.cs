using System.Linq.Expressions;

using FluentAssertions;

namespace ExRam.Gremlinq.Core.Tests
{
    public class ExpressionExtensionsTests
    {
        private readonly int _age = 25;

        [Fact]
        public void Add_of_nullable_ints()
        {
            int? value = 1;

            ValueOf(() => value + 1)
                .Should()
                .Be(2);
        }

        [Fact]
        public void Subtract_through_operator_method()
        {
            var now = DateTime.UtcNow;
            var hour = TimeSpan.FromHours(1);

            ValueOf(() => now - hour)
                .Should()
                .Be(now - hour);
        }

        [Fact]
        public void Divide_by_zero_throws()
        {
            var zero = 0;

            FluentActions
                .Invoking(() => ValueOf(() => _age / zero))
                .Should()
                .Throw<DivideByZeroException>();
        }

        private static object? ValueOf<T>(Expression<Func<T>> expression) => expression.Body.GetValue();
    }
}
