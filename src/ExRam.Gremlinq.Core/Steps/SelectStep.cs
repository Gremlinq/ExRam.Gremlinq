using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class SelectStep : Step
    {
        public SelectStep(StepLabel stepLabel) : this(ImmutableArray.Create(stepLabel))
        {
        }

        public SelectStep(ImmutableArray<StepLabel> stepLabels) : base()
        {
            StepLabels = stepLabels;
        }

        public ImmutableArray<StepLabel> StepLabels { get; }
    }
}
