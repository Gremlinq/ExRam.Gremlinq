namespace ExRam.Gremlinq
{
    public sealed class AggregateStep : Step
    {
        public AggregateStep(StepLabel stepLabel)
        {
            StepLabel = stepLabel;
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public StepLabel StepLabel { get; }
    }

    public sealed class TailStep : Step
    {
        public TailStep(int count)
        {
            Count = count;
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public int Count { get; }
    }
}
