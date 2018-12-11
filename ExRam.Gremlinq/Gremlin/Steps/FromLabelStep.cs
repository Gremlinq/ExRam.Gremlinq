namespace ExRam.Gremlinq
{
    public sealed class FromLabelStep : Step
    {
        public FromLabelStep(StepLabel stepLabel)
        {
            StepLabel = stepLabel;
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public StepLabel StepLabel { get; }
    }
}
