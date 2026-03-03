using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>select()</c> step that selects a labeled step by its <see cref="StepLabel"/>.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#select-step">Reference Documentation - Select Step</seealso>
    public sealed class SelectStepLabelStep : Step
    {
        /// <summary>Initializes a new instance of <see cref="SelectStepLabelStep"/> with a single step label.</summary>
        /// <param name="stepLabel">The step label to select.</param>
        public SelectStepLabelStep(StepLabel stepLabel) : this(ImmutableArray.Create(stepLabel))
        {
            ArgumentNullException.ThrowIfNull(stepLabel);

        }

        /// <summary>Initializes a new instance of <see cref="SelectStepLabelStep"/> with the specified step labels.</summary>
        /// <param name="stepLabels">The step labels to select.</param>
        public SelectStepLabelStep(ImmutableArray<StepLabel> stepLabels)
        {
            StepLabels = stepLabels;
        }

        /// <summary>Gets the step labels to select.</summary>
        public ImmutableArray<StepLabel> StepLabels { get; }
    }
}
