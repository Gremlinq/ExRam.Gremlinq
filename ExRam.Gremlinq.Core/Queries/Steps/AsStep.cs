namespace ExRam.Gremlinq.Core
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
