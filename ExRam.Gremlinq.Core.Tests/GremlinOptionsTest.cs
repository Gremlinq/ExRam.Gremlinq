using FluentAssertions;
using Xunit;

namespace ExRam.Gremlinq.Core.Tests
{
    public class GremlinOptionsTest
    {
        [Fact]
        public void SetValue_works_on_default()
        {
            var options = default(GremlinqOptions);

            options = options.SetValue(GremlinqOption.FilterLabelsVerbosity, FilterLabelsVerbosity.Minimum);

            options.GetValue(GremlinqOption.FilterLabelsVerbosity)
                .Should()
                .Be(FilterLabelsVerbosity.Minimum);
        }

        [Fact]
        public void Value_can_be_changed()
        {
            var options = default(GremlinqOptions);

            options = options.SetValue(GremlinqOption.FilterLabelsVerbosity, FilterLabelsVerbosity.Minimum);

            options.GetValue(GremlinqOption.FilterLabelsVerbosity)
                .Should()
                .Be(FilterLabelsVerbosity.Minimum);

            options = options.SetValue(GremlinqOption.FilterLabelsVerbosity, FilterLabelsVerbosity.Maximum);

            options.GetValue(GremlinqOption.FilterLabelsVerbosity)
                .Should()
                .Be(FilterLabelsVerbosity.Maximum);
        }

        [Fact]
        public void Two_values_can_be_set()
        {
            var options = default(GremlinqOptions);

            options = options.SetValue(GremlinqOption.FilterLabelsVerbosity, FilterLabelsVerbosity.Minimum);

            options.GetValue(GremlinqOption.FilterLabelsVerbosity)
                .Should()
                .Be(FilterLabelsVerbosity.Minimum);

            options = options.SetValue(GremlinqOption.DisabledTextPredicates, DisabledTextPredicates.Containing);

            options.GetValue(GremlinqOption.DisabledTextPredicates)
                .Should()
                .Be(DisabledTextPredicates.Containing);
        }
    }
}
