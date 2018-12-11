namespace ExRam.Gremlinq
{
    public sealed class FilterStep : Step
    {
        public FilterStep(Lambda lambda)
        {
            Lambda = lambda;
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Lambda Lambda { get; }
    }
}