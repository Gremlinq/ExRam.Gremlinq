using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{

    public sealed class SelectStepLabelStep : Step
    {
        public SelectStepLabelStep(StepLabel stepLabel) : this(ImmutableArray.Create(stepLabel))
        {
        }

        public SelectStepLabelStep(ImmutableArray<StepLabel> stepLabels) : base()
        {
            StepLabels = stepLabels;
        }

        public ImmutableArray<StepLabel> StepLabels { get; }
    }
}
