namespace ExRam.Gremlinq
{
    public sealed class FoldStep : Step
    {
        public static readonly FoldStep Instance = new FoldStep();

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
