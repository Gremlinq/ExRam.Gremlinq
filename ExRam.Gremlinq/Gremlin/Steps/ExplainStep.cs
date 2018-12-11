namespace ExRam.Gremlinq
{
    public sealed class ExplainStep : Step
    {
        public static readonly ExplainStep Instance = new ExplainStep();

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
