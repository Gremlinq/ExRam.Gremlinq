namespace ExRam.Gremlinq.Core
{
    public sealed class FromLabelStep : Step
    {
        public FromLabelStep(StepLabel stepLabel)
        {
            StepLabel = stepLabel;
        }

        public StepLabel StepLabel { get; }
    }
}
