namespace ExRam.Gremlinq
{
    public sealed class SelectStep : Step
    {
        public SelectStep(params StepLabel[] stepLabels)
        {
            StepLabels = stepLabels;
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public StepLabel[] StepLabels { get; }
    }
}
