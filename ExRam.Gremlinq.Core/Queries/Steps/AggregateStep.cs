namespace ExRam.Gremlinq.Core
{
    public sealed class AggregateStep : Step
    {
        public AggregateStep(StepLabel stepLabel)
        {
            StepLabel = stepLabel;
        }

        public StepLabel StepLabel { get; }
    }
}
