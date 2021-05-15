namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class AsStep : Step
    {
        public AsStep(StepLabel stepLabel) : base()
        {
            StepLabel = stepLabel;
        }

        public StepLabel StepLabel { get; }
    }
}
