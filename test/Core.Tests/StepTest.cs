using System.Collections.Immutable;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;
using FluentAssertions;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Tests
{
    public class StepTest
    {
        [Fact]
        public void HasLabelStep_rejects_empty_labels() => new Action(() => new HasLabelStep(ImmutableArray<string>.Empty))
            .Should()
            .ThrowExactly<ArgumentException>()
            .WithMessage("labels may not be empty.");

        [Fact]
        public void RangeStep_rejects_an_upper_bound_below_minus_one() => new Action(() => new RangeStep(0, -2, Scope.Global))
            .Should()
            .ThrowExactly<ArgumentException>();

        [Fact]
        public void ByKeyStep_of_T_key_becomes_a_key_step() => new ProjectStep.ByKeyStep(T.Key)
            .ToByTraversalStep()
            .Traversal[0]
            .Should()
            .BeSameAs(KeyStep.Instance);

        [Fact]
        public void ByKeyStep_of_T_value_becomes_a_value_step() => new ProjectStep.ByKeyStep(T.Value)
            .ToByTraversalStep()
            .Traversal[0]
            .Should()
            .BeSameAs(ValueStep.Instance);

        [Fact]
        public void Project_rejects_fewer_by_steps_than_projections() => Projection.Empty
            .Invoking(_ => _.Project(new ProjectStep(["a", "b"]), new ProjectStep.ByStep[] { new ProjectStep.ByKeyStep("a") }))
            .Should()
            .ThrowExactly<ArgumentException>()
            .WithMessage("projectStep must have the same number of projections as there are steps in bySteps.");

        [Fact]
        public void Project_maps_a_by_key_step_to_an_empty_projection() => Projection.Empty
            .Project(new ProjectStep(["a"]), new ProjectStep.ByStep[] { new ProjectStep.ByKeyStep("a") })
            .Select(["a"])
            .Should()
            .BeSameAs(Projection.Empty);
    }
}
