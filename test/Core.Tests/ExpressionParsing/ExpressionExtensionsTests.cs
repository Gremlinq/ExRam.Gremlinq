using System.Linq.Expressions;

using FluentAssertions;

namespace ExRam.Gremlinq.Core.Tests
{
    public class ExpressionExtensionsTests
    {
        private readonly int _age = 25;

        [Fact]
        public void Convert_boxes() => ValueOf(() => (object)_age)
            .Should()
            .Be(25);

        [Fact]
        public void Convert_wraps_into_nullable() => ValueOf(() => (int?)_age)
            .Should()
            .Be(25);

        [Fact]
        public void Convert_to_base_type_keeps_the_object()
        {
            var name = new string('a', 3);

            ValueOf(() => (object)name)
                .Should()
                .BeSameAs(name);
        }

        [Fact]
        public void Convert_widens() => ValueOf(() => (long)_age)
            .Should()
            .BeOfType<long>()
            .Which
            .Should()
            .Be(25L);

        [Fact]
        public void Convert_of_enum_to_underlying_type() => ValueOf(() => (int)DayOfWeek.Wednesday)
            .Should()
            .Be(3);

        [Fact]
        public void Convert_of_underlying_type_to_enum() => ValueOf(() => (DayOfWeek)(_age % 7))
            .Should()
            .Be(DayOfWeek.Thursday);

        [Fact]
        public void Convert_of_null_to_value_type_throws()
        {
            object? value = null;

            FluentActions
                .Invoking(() => ValueOf(() => (int)value!))
                .Should()
                .Throw<NullReferenceException>();
        }

        [Fact]
        public void Convert_checked_throws_on_overflow()
        {
            var value = long.MaxValue;

            FluentActions
                .Invoking(() => ValueOf(() => checked((int)value)))
                .Should()
                .Throw<OverflowException>();
        }

        [Fact]
        public void Conditional_takes_the_branch()
        {
            var flag = true;

            ValueOf(() => flag ? "yes" : "no")
                .Should()
                .Be("yes");

            flag = false;

            ValueOf(() => flag ? "yes" : "no")
                .Should()
                .Be("no");
        }

        [Fact]
        public void ArrayIndex()
        {
            var ids = new[] { 1, 2, 3 };

            ValueOf(() => ids[1])
                .Should()
                .Be(2);
        }

        [Fact]
        public void Add_of_ints() => ValueOf(() => _age + 1)
            .Should()
            .Be(26);

        [Fact]
        public void Add_of_ints_wraps()
        {
            var value = int.MaxValue;

            ValueOf(() => value + 1)
                .Should()
                .Be(int.MinValue);
        }

        [Fact]
        public void Subtract_of_longs()
        {
            var value = 5L;

            ValueOf(() => value - 2L)
                .Should()
                .Be(3L);
        }

        [Fact]
        public void Multiply_of_doubles()
        {
            var value = 1.5;

            ValueOf(() => value * 2.0)
                .Should()
                .Be(3.0);
        }

        [Fact]
        public void Negate_of_int() => ValueOf(() => -_age)
            .Should()
            .Be(-25);

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
