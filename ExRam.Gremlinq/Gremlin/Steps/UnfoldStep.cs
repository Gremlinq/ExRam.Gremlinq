namespace ExRam.Gremlinq
{
    public sealed class UnfoldStep : Step
    {
        public static readonly UnfoldStep Instance = new UnfoldStep();

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
