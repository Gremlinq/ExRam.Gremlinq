namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class CapStep : Step
    {
        public CapStep(StepLabel stepLabel) : base()
        {
            StepLabel = stepLabel;
        }

        public StepLabel StepLabel { get; }
    }
}
