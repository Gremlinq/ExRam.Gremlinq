namespace ExRam.Gremlinq.Core
{
    public sealed class ToLabelStep : Step
    {
        public ToLabelStep(StepLabel stepLabel)
        {
            StepLabel = stepLabel;
        }

        public StepLabel StepLabel { get; }
    }
}
