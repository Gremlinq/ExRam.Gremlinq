namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>as()</c> step modulator that labels the current step for later reference.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#as-step">Reference Documentation - As Step</seealso>
    public sealed class AsStep : Step
    {
        /// <summary>Initializes a new instance of <see cref="AsStep"/> with the specified step label.</summary>
        /// <param name="stepLabel">The label to assign to the current step.</param>
        public AsStep(StepLabel stepLabel)
        {
            ArgumentNullException.ThrowIfNull(stepLabel);

            StepLabel = stepLabel;
        }

        /// <summary>Gets the step label assigned to the current step.</summary>
        public StepLabel StepLabel { get; }
    }
}
