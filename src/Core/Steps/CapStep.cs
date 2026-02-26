namespace ExRam.Gremlinq.Core.Steps
{
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
