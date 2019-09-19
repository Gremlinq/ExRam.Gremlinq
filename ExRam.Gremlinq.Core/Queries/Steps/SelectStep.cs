namespace ExRam.Gremlinq.Core
{
    public sealed class SelectStep : Step
    {
        public SelectStep(params StepLabel[] stepLabels)
        {
            StepLabels = stepLabels;
        }

        public StepLabel[] StepLabels { get; }
    }
}
