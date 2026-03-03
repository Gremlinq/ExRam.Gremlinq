using FluentAssertions;

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

            (stepLabel == castStepLabel).Should().BeTrue();
        }

        [Fact]
        public void Inequality()
        {
            var stepLabel1 = new StepLabel<object>();
            var stepLabel2 = new StepLabel<object>();

            (stepLabel1 != stepLabel2).Should().BeTrue();
        }

        [Fact]
        public void Equals_null()
        {
            var stepLabel1 = new StepLabel<object>();

            (stepLabel1.Equals(null)).Should().BeFalse();
        }

        [Fact]
        public void Equals_identity()
        {
            var stepLabel1 = new StepLabel<object>();

            (stepLabel1.Equals(stepLabel1)).Should().BeTrue();
        }

        [Fact]
        public void Equals_other()
        {
            var stepLabel1 = new StepLabel<object>();
            var stepLabel2 = new StepLabel<object>();

            (stepLabel1.Equals(stepLabel2)).Should().BeFalse();
        }

        [Fact]
        public void Identity_is_preserved_through_when_created_from_string()
        {
            StepLabel stepLabel1 = "stepLabel";
            StepLabel stepLabel2 = "stepLabel";

            stepLabel1.Should().Be(stepLabel2);
        }
    }
}
