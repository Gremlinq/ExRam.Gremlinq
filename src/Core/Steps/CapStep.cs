namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>cap()</c> step that retrieves the contents of a side-effect by its step label.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#cap-step">Reference Documentation - Cap Step</seealso>
    public sealed class CapStep : Step
    {
        /// <summary>Initializes a new instance of <see cref="CapStep"/> with the specified step label.</summary>
        /// <param name="stepLabel">The step label of the side-effect to retrieve.</param>
        public CapStep(StepLabel stepLabel)
        {
            ArgumentNullException.ThrowIfNull(stepLabel);

            StepLabel = stepLabel;
        }

        /// <summary>Gets the step label of the side-effect to retrieve.</summary>
        public StepLabel StepLabel { get; }
    }
}
