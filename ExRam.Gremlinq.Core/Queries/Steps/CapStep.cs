namespace ExRam.Gremlinq.Core
{
    public sealed class CapStep : Step
    {
        public CapStep(StepLabel stepLabel)
        {
            StepLabel = stepLabel;
        }

        public StepLabel StepLabel { get; }
    }
}