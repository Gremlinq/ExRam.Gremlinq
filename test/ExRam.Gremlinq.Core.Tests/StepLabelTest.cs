using FluentAssertions;
using Xunit;

namespace ExRam.Gremlinq.Core.Tests
{
    public class StepLabelTest
    {
        [Fact]
        public void Identity_is_preserved_through_Equals_by_Cast()
        {
            var stepLabel = new StepLabel<object>();
            var castStepLabel = stepLabel.Cast<string>();

            stepLabel.Should().Be(castStepLabel);
        }

        [Fact]
        public void Identity_is_preserved_through_operator_by_Cast()
        {
            var stepLabel = new StepLabel<object>();
            var castStepLabel = stepLabel.Cast<string>();

            ((StepLabel)stepLabel == castStepLabel).Should().BeTrue();
        }
    }
}
