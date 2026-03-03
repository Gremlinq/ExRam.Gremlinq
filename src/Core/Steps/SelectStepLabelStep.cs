using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>select()</c> step that selects a labeled step by its <see cref="StepLabel"/>.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#select-step">Reference Documentation - Select Step</seealso>
    public sealed class SelectStepLabelStep : Step
    {
        public SelectStepLabelStep(StepLabel stepLabel) : this(ImmutableArray.Create(stepLabel))
        {
            ArgumentNullException.ThrowIfNull(stepLabel);

        }

        public SelectStepLabelStep(ImmutableArray<StepLabel> stepLabels)
        {
            StepLabels = stepLabels;
        }

        public ImmutableArray<StepLabel> StepLabels { get; }
    }
}
