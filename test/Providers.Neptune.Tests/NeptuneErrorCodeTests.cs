using FluentAssertions;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public sealed class NeptuneErrorCodeTests
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
    }
}
