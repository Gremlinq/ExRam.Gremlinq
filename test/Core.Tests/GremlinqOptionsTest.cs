using FluentAssertions;

namespace ExRam.Gremlinq.Core.Tests
{
    public class GremlinqOptionsTest
    {
        private static readonly GremlinqOption<string> TestOption = GremlinqOption.Create("default");
        private static readonly GremlinqOption<int> IntOption = GremlinqOption.Create(42);

        [Fact]
        public void GetValue_returns_default_when_not_set()
        {
            GremlinqOptions.Empty
                .GetValue(TestOption)
                .Should()
                .Be("default");
        }

        [Fact]
        public void SetValue_and_GetValue()
        {
            GremlinqOptions.Empty
                .SetValue(TestOption, "custom")
                .GetValue(TestOption)
                .Should()
                .Be("custom");
        }

        [Fact]
        public void Contains_returns_false_when_not_set()
        {
            GremlinqOptions.Empty
                .Contains(TestOption)
                .Should()
                .BeFalse();
        }

        [Fact]
        public void Contains_returns_true_when_set()
        {
            GremlinqOptions.Empty
                .SetValue(TestOption, "custom")
                .Contains(TestOption)
                .Should()
                .BeTrue();
        }

        [Fact]
        public void ConfigureValue_applies_transformation_to_default()
        {
            GremlinqOptions.Empty
                .ConfigureValue(TestOption, static v => v + "_configured")
                .GetValue(TestOption)
                .Should()
                .Be("default_configured");
        }

        [Fact]
        public void ConfigureValue_applies_transformation_to_existing()
        {
            GremlinqOptions.Empty
                .SetValue(TestOption, "existing")
                .ConfigureValue(TestOption, static v => v + "_configured")
                .GetValue(TestOption)
                .Should()
                .Be("existing_configured");
        }

        [Fact]
        public void Remove_option()
        {
            GremlinqOptions.Empty
                .SetValue(TestOption, "custom")
                .Remove(TestOption)
                .Contains(TestOption)
                .Should()
                .BeFalse();
        }

        [Fact]
        public void Remove_returns_default_after_removal()
        {
            GremlinqOptions.Empty
                .SetValue(TestOption, "custom")
                .Remove(TestOption)
                .GetValue(TestOption)
                .Should()
                .Be("default");
        }

        [Fact]
        public void SetValue_overwrites_previous()
        {
            GremlinqOptions.Empty
                .SetValue(IntOption, 100)
                .SetValue(IntOption, 200)
                .GetValue(IntOption)
                .Should()
                .Be(200);
        }

        [Fact]
        public void Multiple_options_are_independent()
        {
            var options = GremlinqOptions.Empty
                .SetValue(TestOption, "custom")
                .SetValue(IntOption, 99);

            options.GetValue(TestOption)
                .Should()
                .Be("custom");

            options.GetValue(IntOption)
                .Should()
                .Be(99);
        }
    }
}
