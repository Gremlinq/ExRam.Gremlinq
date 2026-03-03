namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>cap()</c> step that retrieves the contents of a side-effect by its step label.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#cap-step">Reference Documentation - Cap Step</seealso>
    public sealed class CapStep : Step
    {
        public CapStep(StepLabel stepLabel)
        {
            ArgumentNullException.ThrowIfNull(stepLabel);

            StepLabel = stepLabel;
        }

        public StepLabel StepLabel { get; }
    }
}
