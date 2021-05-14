using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class SelectStep : Step
    {
        public SelectStep(ImmutableArray<StepLabel> stepLabels) : base()
        {
            StepLabels = stepLabels;
        }

        public ImmutableArray<StepLabel> StepLabels { get; }
    }
}
