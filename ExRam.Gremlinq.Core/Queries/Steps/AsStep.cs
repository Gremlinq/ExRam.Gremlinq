namespace ExRam.Gremlinq.Core
{
    public sealed class AsStep : Step
    {
        public AsStep(StepLabel[] stepLabels)
        {
            StepLabels = stepLabels;
        }

        public StepLabel[] StepLabels { get; }
    }
}
