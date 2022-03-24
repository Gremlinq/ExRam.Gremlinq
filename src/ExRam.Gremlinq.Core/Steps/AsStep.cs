namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class AsStep : Step
    {
        public AsStep(StepLabel stepLabel)
        {
            StepLabel = stepLabel;
        }

        public StepLabel StepLabel { get; }
    }
}
