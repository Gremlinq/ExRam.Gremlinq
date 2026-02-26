namespace ExRam.Gremlinq.Core.Steps
{
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
