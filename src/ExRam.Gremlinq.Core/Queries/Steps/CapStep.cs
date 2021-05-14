namespace ExRam.Gremlinq.Core
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
