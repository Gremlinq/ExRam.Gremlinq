using FluentAssertions;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public class NeptuneErrorCodeTests
    {
        [Fact]
        public void Equality()
        {
            var value1 = "Value";
            var value2 = "Value123".Substring(0, 5);

            ReferenceEquals(value1, value2).Should().BeFalse();

            var code1 = NeptuneErrorCode.From(value1);
            var code2 = NeptuneErrorCode.From(value2);

            code1.Equals(code2).Should().BeTrue();
            (code1 == code2).Should().BeTrue();
        }

        [Fact]
        public void Inequality()
        {
            var code1 = NeptuneErrorCode.From("Code1");
            var code2 = NeptuneErrorCode.From("Code2");

            (code1 != code2).Should().BeTrue();
            (code1 == code2).Should().BeFalse();
        }

        [Fact]
        public void GetHashCode_equal_for_equal_codes()
        {
            var code1 = NeptuneErrorCode.From("Test");
            var code2 = NeptuneErrorCode.From("Test");

            code1.GetHashCode().Should().Be(code2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_different_for_different_codes()
        {
            var code1 = NeptuneErrorCode.From("Code1");
            var code2 = NeptuneErrorCode.From("Code2");

            code1.GetHashCode().Should().NotBe(code2.GetHashCode());
        }

        [Fact]
        public void Equals_object_returns_false_for_non_NeptuneErrorCode()
        {
            var code = NeptuneErrorCode.From("Test");

            code.Equals("Test").Should().BeFalse();
            code.Equals(null).Should().BeFalse();
        }

        [Fact]
        public void Code_property_returns_value()
        {
            NeptuneErrorCode.AccessDeniedException.Code
                .Should().Be("AccessDeniedException");
        }

        [Fact]
        public void Default_struct_Code_throws()
        {
            var code = default(NeptuneErrorCode);

            code
                .Invoking(c => _ = c.Code)
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Fact]
        public void From_null_throws()
        {
            FluentActions.Invoking(() => NeptuneErrorCode.From(null!))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void From_empty_throws()
        {
            FluentActions.Invoking(() => NeptuneErrorCode.From(""))
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public void Static_error_codes_are_initialized()
        {
            NeptuneErrorCode.BadRequestException.Code.Should().Be("BadRequestException");
            NeptuneErrorCode.ThrottlingException.Code.Should().Be("ThrottlingException");
            NeptuneErrorCode.InternalFailureException.Code.Should().Be("InternalFailureException");
        }
    }
}
