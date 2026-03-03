namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>as()</c> step modulator that labels the current step for later reference.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#as-step">Reference Documentation - As Step</seealso>
    public sealed class AsStep : Step
    {
        public AsStep(StepLabel stepLabel)
        {
            ArgumentNullException.ThrowIfNull(stepLabel);

            StepLabel = stepLabel;
        }

        public StepLabel StepLabel { get; }
    }
}
