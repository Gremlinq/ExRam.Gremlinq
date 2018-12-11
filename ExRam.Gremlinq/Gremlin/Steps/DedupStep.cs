namespace ExRam.Gremlinq
{
    public sealed class DedupStep : Step
    {
        public static readonly DedupStep Instance = new DedupStep();

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
